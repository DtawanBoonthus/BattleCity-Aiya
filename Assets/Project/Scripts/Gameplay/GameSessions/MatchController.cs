using System;
using System.Threading;
using System.Threading.Tasks;
using BC.Shared.GameSessions;
using BC.Shared.Networks;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VitalRouter;

namespace BC.Gameplay;

public class MatchController : IStartable, IDisposable
{
    [Inject] private readonly ICommandPublisher publisher = null!;
    [Inject] private readonly INetworkConfig networkConfig = null!;
    [Inject] private readonly INetworkLifecycle networkLifecycle = null!;
    [Inject] private readonly INetworkRoomService roomService = null!;
    [Inject] private readonly INetworkContext networkContext = null!;
    [Inject] private readonly MatchControllerRouter matchControllerRouter = null!;

    private CompositeDisposable? disposables;
    private const string LOG_PREFIX = $"[{nameof(MatchController)}]";

    public void Start()
    {
        HandlePlayerCountForGameStart();
    }

    public void Dispose()
    {
        disposables?.Dispose();
    }

    private void HandlePlayerCountForGameStart()
    {
        disposables ??= new CompositeDisposable();

        matchControllerRouter.OnGameModeCommand.SubscribeAwait(OnGameModeCommand).AddTo(disposables);

        roomService.PlayerCount
            .Where(_ => networkContext.IsServer)
            .Subscribe(count =>
            {
                Debug.Log($"{LOG_PREFIX} PlayerCount = {count}");

                if (count < networkConfig.MaxConnections)
                {
                    return;
                }

                StartGameAsync().Forget();
            }).AddTo(disposables);
    }

    private async ValueTask OnGameModeCommand(GameMode gameMode, CancellationToken cancellationToken)
    {
        var connectionStatus = gameMode switch
        {
            GameMode.Host => await networkLifecycle.StartHostAsync(),
            GameMode.Client => await networkLifecycle.StartClientAsync(networkConfig.Address),
            _ => ConnectRoomStatus.Failed
        };

        if (connectionStatus == ConnectRoomStatus.Failed)
        {
            Debug.LogError($"{LOG_PREFIX} Failed to connect to server.");
            return;
        }

        ConnectGameSuccessAsync().Forget();

        if (networkContext.IsServer)
        {
            StartServerGameAsync().Forget();
        }
    }

    private async UniTask StartServerGameAsync()
    {
        await publisher.PublishAsync(new GameStartServerCommand());
    }

    private async UniTask StartGameAsync()
    {
        await publisher.PublishAsync(new GameStartCommand());
    }

    private async UniTask ConnectGameSuccessAsync()
    {
        await publisher.PublishAsync(new ConnectGameSuccess());
    }
}