using System;
using BC.Shared.Networks;
using Mirage;
using R3;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Networks;

public class MirageNetworkRoomService : INetworkRoomService, IStartable, IDisposable
{
    [Inject] private readonly NetworkManager manager = null!;

    private readonly ReactiveProperty<int> playerCount = new();
    public ReadOnlyReactiveProperty<int> PlayerCount => playerCount;

    public void Start()
    {
        playerCount.Value = GetCurrentCount();

        manager.Server.Connected.AddListener(OnConnected);
        manager.Server.Disconnected.AddListener(OnDisconnected);
    }

    public void Dispose()
    {
        manager.Server.Connected.RemoveListener(OnConnected);
        manager.Server.Disconnected.RemoveListener(OnDisconnected);
        playerCount.Dispose();
    }

    private void OnConnected(INetworkPlayer player)
    {
        playerCount.Value = GetCurrentCount();
    }

    private void OnDisconnected(INetworkPlayer player)
    {
        playerCount.Value = GetCurrentCount();
    }

    private int GetCurrentCount()
    {
        return manager.Server.AllPlayers.Count;
    }
}