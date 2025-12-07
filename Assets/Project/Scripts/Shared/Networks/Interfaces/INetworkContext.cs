using Mirage;

namespace BC.Shared.Networks;

public interface INetworkContext
{
    NetworkManager NetworkManager { get; }
    NetworkServer Server { get; }
    NetworkClient Client { get; }
    NetworkSceneManager NetworkSceneManager { get; }
    ServerObjectManager ServerObjectManager { get; }
    ClientObjectManager ClientObjectManager { get; }
    bool IsServer { get; }
    bool IsClient { get; }
    bool IsHost { get; }
}