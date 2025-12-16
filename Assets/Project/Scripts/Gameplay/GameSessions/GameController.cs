using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BC.Gameplay.Configs;
using BC.Gameplay.Tanks;
using BC.Shared.Spawners;
using Cysharp.Threading.Tasks;
using Mirage;
using R3;
using UnityEngine;
using VContainer;
using VitalRouter;
using ZLinq;

namespace BC.Gameplay
{
    public class GameController : NetworkBehaviour
    {
        [Inject] private readonly SpawnPosition spawnLocations = null!;
        [Inject] private readonly GameControllerRouter router = null!;
        [Inject] private readonly IGameplayConfig gameplayConfig = null!;
        [Inject] private readonly ISpawnService<MirageNet> spawnService = null!;
        [Inject] private readonly NetworkManager networkManager = null!;
        [Inject] private readonly ICommandPublisher publisher = null!;

        private CompositeDisposable? disposables;
        private CompositeDisposable? destroyDisposable;
        private const string LOG_PREFIX = $"[{nameof(GameController)}]";
        private readonly List<GameObject> tankObjects = new();

        private void Start()
        {
            destroyDisposable ??= new CompositeDisposable();
            router.OnGameStart.SubscribeAwait(OnGameStartCommand).AddTo(destroyDisposable);
            router.TankDead.SubscribeAwait(OnTankDeadCommand).AddTo(destroyDisposable);
        }

        private void OnDestroy()
        {
            destroyDisposable?.Dispose();
            destroyDisposable = null;
        }

        private void OnEnable()
        {
            disposables ??= new CompositeDisposable();
        }

        private void OnDisable()
        {
            disposables?.Dispose();
            disposables = null;
        }

        [Server]
        private async ValueTask OnGameStartCommand(Unit _, CancellationToken cancellationToken)
        {
            if (!IsServer)
            {
                return;
            }

            //Hard coded delay
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
            await StartGameSequence();
        }

        private async UniTask StartGameSequence()
        {
            if (router.GameStartCountdown == null)
            {
                Debug.LogError($"{LOG_PREFIX} GameStartCountdown is null.");
                return;
            }

            await router.GameStartCountdown.StartCountdownAsync(gameplayConfig.StartGameDurationSecond);
            await SpawnTankAsync();
        }

        [Server]
        private UniTask SpawnTankAsync()
        {
            if (!IsServer)
            {
                return UniTask.CompletedTask;
            }

            var players = networkManager.Server.AllPlayers
                .Where(p => p.Identity != null)
                .ToList();

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var spawnPos = spawnLocations.SpawnPositions[i];

                var tankObj = spawnService.Spawn(gameplayConfig.TankPrefab, spawnPos.position, Quaternion.identity, player);
                var tankRender = tankObj.GetComponent<TankRender>();
                tankRender.RPC_SetSprite(i);
                tankObjects.Add(tankObj);
            }

            return UniTask.CompletedTask;
        }

        [Server]
        private async ValueTask OnTankDeadCommand((uint winId, uint loseId) tankDeadCommand, CancellationToken cancellationToken)
        {
            if (!IsServer)
            {
                return;
            }

            var tankNetIds = tankObjects.Select(x => x.GetComponent<TankMovement>().NetId).ToArray();
            RPC_SentResultGame(tankDeadCommand.winId, tankDeadCommand.loseId, tankNetIds);
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
            await RestartGame();
        }

        [ClientRpc]
        private void RPC_SentResultGame(uint winId, uint loseId, uint[] tankNetIds)
        {
            foreach (var netId in tankNetIds)
            {
                if (!Client.World.TryGetIdentity(netId, out var identity))
                {
                    continue;
                }

                var tank = identity.GetComponent<TankMovement>();
                if (tank != null)
                {
                    tank.gameObject.SetActive(false);
                }
            }

            var localPlayerNetId = Client.Player.Identity.NetId;
            var statusEndGame =
                localPlayerNetId == winId ? "Win!" :
                localPlayerNetId == loseId ? "Lose!" :
                string.Empty;

            PublishResultEndGame(statusEndGame).Forget();
        }

        private async UniTask PublishResultEndGame(string status)
        {
            await publisher.PublishAsync(new ResultEndGameCommand(status));
        }

        [Server]
        private async UniTask RestartGame()
        {
            foreach (var tankObj in tankObjects)
            {
                if (tankObj == null)
                {
                    continue;
                }

                spawnService.Despawn(tankObj);
            }

            tankObjects.Clear();

            PublishRestartGameCommand();
            await UniTask.Delay(1500);
            await StartGameSequence();
        }

        [ClientRpc]
        private void PublishRestartGameCommand()
        {
            publisher.PublishAsync(new RestartGameCommand());
        }
    }
}