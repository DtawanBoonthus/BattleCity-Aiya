using System.Collections.Generic;
using BC.Shared.Networks;
using Mirage;
using VContainer;
using ZLinq;

namespace BC.Core.Networks;

public class MirageNetworkContext : INetworkContext
{
    [Inject] private readonly NetworkManager networkManager = null!;

    public bool IsServer => networkManager.Server.Active;

    public bool IsClient => networkManager.Client.Active;

    public bool IsHost => IsServer && IsClient;

    public bool IsConnected => networkManager.Client.IsConnected;

    public uint LocalPlayerId => networkManager.Client.Player.Identity.NetId;

    public IReadOnlyList<uint> ConnectedPlayers =>
        networkManager.Server.AllPlayers
            .Select(player => player.Identity.NetId)
            .ToList();
}