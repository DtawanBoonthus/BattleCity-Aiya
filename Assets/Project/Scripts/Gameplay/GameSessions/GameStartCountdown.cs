using Cysharp.Threading.Tasks;
using Mirage;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace BC.Gameplay
{
    public class GameStartCountdown : NetworkBehaviour, IGameStartCountdown
    {
        [Inject] private readonly ICommandPublisher publisher = null!;
        
        [Server]
        public async UniTask StartCountdownAsync(int seconds)
        {
            for (int i = seconds; i >= 0; i--)
            {
                RpcTick(i);
                await UniTask.Delay(1000);
            }
        }

        [ClientRpc]
        private void RpcTick(int secondsLeft)
        {
            PublishGameCountdownTick(secondsLeft).Forget();
        }

        private async UniTask PublishGameCountdownTick(int secondsLeft)
        {
            Debug.Log($"Countdown: {secondsLeft}");
            await publisher.PublishAsync(new GameStartCountdownTickCommand(secondsLeft));
        }
    }
}