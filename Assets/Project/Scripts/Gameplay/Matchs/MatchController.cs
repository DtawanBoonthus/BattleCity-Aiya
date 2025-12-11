using System;
using System.Threading;
using BC.Shared.Networks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BC.Gameplay.Matchs;

public sealed class MatchController : IAsyncStartable, IDisposable
{
    [Inject] private readonly INetworkConfig networkConfig = null!;
    [Inject] private readonly INetworkLifecycle networkLifecycle = null!;
    [Inject] private readonly INetworkRoomService roomService = null!;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        var startClient = await networkLifecycle.StartClient(networkConfig.Address);

        Debug.Log($"[MatchController] StartClient: {startClient}");

        if (startClient == ConnectRoomStatus.Failed)
        {
            networkLifecycle.StopClient();
            var startHost = await networkLifecycle.StartHost();
            Debug.Log($"[MatchController] StartHost: {startHost}");
        }
    }

    public void Dispose()
    {
    }
}