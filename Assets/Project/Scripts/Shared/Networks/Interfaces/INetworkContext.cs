using System.Collections.Generic;

namespace BC.Shared.Networks;

public interface INetworkContext
{
    bool IsServer { get; }
    bool IsClient { get; }
    bool IsHost { get; }
    bool IsConnected { get; }
    uint LocalPlayerId { get; }
    IReadOnlyList<uint> ConnectedPlayers { get; }
}