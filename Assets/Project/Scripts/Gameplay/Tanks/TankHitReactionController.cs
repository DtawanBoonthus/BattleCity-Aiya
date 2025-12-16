using System;
using System.Threading;
using BC.Gameplay.Damageable;
using Cysharp.Threading.Tasks;
using Mirage;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace BC.Gameplay.Tanks
{
    public class TankHitReactionController : NetworkBehaviour, IStagger, IIFrame
    {
        [Inject] private readonly ICommandPublisher publisher = null!;

        [SyncVar] private bool isStaggered;
        [SyncVar] private bool isIFrame;
        [SyncVar(hook = nameof(OnStaggerTime))] private int staggerTime;
        [SyncVar(hook = nameof(OnIframeTime))]private float iFrameTime;
        public bool IsStaggered => isStaggered;
        public bool IsIFrame => isIFrame;

        private CancellationToken destroyToken;

        private void Awake()
        {
            destroyToken = this.GetCancellationTokenOnDestroy();
        }

        [Server]
        public void Stagger(int staggerTime)
        {
            if (isStaggered)
            {
                return;
            }

            ApplyStaggerAsync(staggerTime).Forget();
        }

        [Server]
        private async UniTaskVoid ApplyStaggerAsync(int totalSeconds)
        {
            isStaggered = true;
            staggerTime = totalSeconds;

            RpcStaggerStart(NetId, totalSeconds);

            try
            {
                while (staggerTime > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: destroyToken);

                    if (!IsServer)
                    {
                        return;
                    }

                    staggerTime--;
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }

            isStaggered = false;
            RpcStaggerEnd(NetId);
        }

        [ClientRpc]
        private void RpcStaggerStart(uint netId, int staggerTime)
        {
            PublishStaggerCommandAsync(netId, staggerTime).Forget();
        }

        [ClientRpc]
        private void RpcStaggerEnd(uint netId)
        {
            PublishEndStaggerCommandAsync(netId).Forget();
        }

        private async UniTask PublishStaggerCommandAsync(uint netId, int staggerTime)
        {
            await publisher.PublishAsync(new StaggerCommand(netId, staggerTime), destroyToken);
        }

        private async UniTask PublishEndStaggerCommandAsync(uint netId)
        {
            await publisher.PublishAsync(new EndStaggerCommand(netId), destroyToken);
        }

        [Server]
        public void IFrame(float iframeTime)
        {
            if (isIFrame)
            {
                return;
            }

            ApplyIFrameAsync(iframeTime).Forget();
        }

        [Server]
        private async UniTaskVoid ApplyIFrameAsync(float totalTime)
        {
            isIFrame = true;

            iFrameTime = totalTime;
            RpcIFrameStart(NetId, totalTime);

            try
            {
                while (iFrameTime > 0f)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: destroyToken);

                    if (!IsServer)
                    {
                        return;
                    }

                    iFrameTime = Mathf.Max(0, iFrameTime - 1f);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }

            isIFrame = false;
            RpcIFrameEnd(NetId);
        }

        [ClientRpc]
        private void RpcIFrameStart(uint netId, float iframeTime)
        {
            PublishIFrameCommandAsync(netId, iframeTime).Forget();
        }

        [ClientRpc]
        private void RpcIFrameEnd(uint netId)
        {
            PublishEndIFrameCommandAsync(netId).Forget();
        }

        private async UniTask PublishIFrameCommandAsync(uint netId, float iframeTime)
        {
            await publisher.PublishAsync(new IFrameCommand(netId, iframeTime), destroyToken);
        }

        private async UniTask PublishEndIFrameCommandAsync(uint netId)
        {
            await publisher.PublishAsync(new EndIFrameCommand(netId), destroyToken);
        }

        private void OnStaggerTime(int oldValue, int newValue)
        {
            if (!IsClient)
            {
                return;
            }

            PublishStaggerCommandAsync(NetId, newValue).Forget();
        }

        private void OnIframeTime(float oldValue, float newValue)
        {
            if (!IsClient)
            {
                return;
            }

            PublishIFrameCommandAsync(NetId, newValue).Forget();
        }
    }
}