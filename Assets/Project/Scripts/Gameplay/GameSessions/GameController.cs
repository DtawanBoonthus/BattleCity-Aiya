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

        private CompositeDisposable? disposables;
        private CompositeDisposable? destroyDisposable;
        private const string LOG_PREFIX = $"[{nameof(GameController)}]";

        private void Start()
        {
            destroyDisposable ??= new CompositeDisposable();
            router.OnGameStart.SubscribeAwait(OnGameStartCommand).AddTo(destroyDisposable);
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

            if (router.GameStartCountdown == null)
            {
                Debug.LogError($"{LOG_PREFIX} GameStartCountdown is null.");
                return;
            }

            //Hard coded delay
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
            await router.GameStartCountdown.StartCountdownAsync(gameplayConfig.StartGameDurationSecond);
            await SpawnTankAsync();
            Debug.Log($"{LOG_PREFIX} Game Started!");
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

                var tankObj = spawnService.Spawn(
                    gameplayConfig.TankPrefab,
                    spawnPos.position,
                    Quaternion.identity,
                    player
                );
            }

            return UniTask.CompletedTask;
        }
    }
}