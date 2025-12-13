using BC.Gameplay.Configs;
using BC.Shared.Networks;
using BC.Shared.Spawners;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace BC.Gameplay;

[Routes]
public partial class GameController
{
    [Inject] private readonly INetworkContext networkContext = null!;
    [Inject] private readonly IGameplayConfig gameplayConfig = null!;
    [Inject] private readonly ISpawnService<MirageNet> spawnService = null!;

    private IGameStartCountdown countdown = null!;

    private const string LOG_PREFIX = $"[{nameof(GameController)}]";

    [Route]
    private UniTask OnGameStartServerCommand(GameStartServerCommand gameStartServerCommand)
    {
        var gameStartCountdown = spawnService.Spawn(gameplayConfig.GameStartCountdownPrefab, Vector3.zero, Quaternion.identity);
        countdown = gameStartCountdown.GetComponent<IGameStartCountdown>();
        return UniTask.CompletedTask;
    }

    [Route]
    private async UniTask OnGameStartCommand(GameStartCommand gameStartCommand)
    {
        if (!networkContext.IsServer)
        {
            return;
        }
        
        //Hard coded delay
        await UniTask.Delay(1000);
        await countdown.StartCountdownAsync(gameplayConfig.StartGameDurationSecond);
        Debug.Log($"{LOG_PREFIX} Game Started!");
    }
}