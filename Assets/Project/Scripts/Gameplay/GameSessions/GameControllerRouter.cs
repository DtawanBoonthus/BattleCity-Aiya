using BC.Gameplay.Configs;
using BC.Shared.Networks;
using BC.Shared.Spawners;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace BC.Gameplay;

[Routes]
public partial class GameControllerRouter
{
    [Inject] private readonly INetworkContext networkContext = null!;
    [Inject] private readonly IGameplayConfig gameplayConfig = null!;
    [Inject] private readonly ISpawnService<MirageNet> spawnService = null!;

    private readonly Subject<Unit> gameStart = new();

    public IGameStartCountdown? GameStartCountdown { get; private set; } = null;
    public Observable<Unit> OnGameStart => gameStart;

    private const string LOG_PREFIX = $"[{nameof(GameControllerRouter)}]";

    [Route]
    private UniTask OnGameStartServerCommand(GameStartServerCommand gameStartServerCommand)
    {
        var gameControllerObj = spawnService.Spawn(gameplayConfig.GameControllerPrefab, Vector3.zero, Quaternion.identity);

        var gameStartCountdown = spawnService.Spawn(gameplayConfig.GameStartCountdownPrefab, Vector3.zero, Quaternion.identity);
        GameStartCountdown = gameStartCountdown.GetComponent<IGameStartCountdown>();
        return UniTask.CompletedTask;
    }

    [Route]
    private UniTask OnGameStartCommand(GameStartCommand gameStartCommand)
    {
        if (!networkContext.IsServer)
        {
            return UniTask.CompletedTask;
        }

        gameStart.OnNext(Unit.Default);
        return UniTask.CompletedTask;
    }
}