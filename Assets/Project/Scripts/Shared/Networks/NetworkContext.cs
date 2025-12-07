using Mirage;

namespace BC.Shared.Networks;

public class NetworkContext : INetworkContext
{
    public NetworkManager NetworkManager { get; }
    public NetworkServer Server { get; }
    public NetworkClient Client { get; }
    public NetworkSceneManager NetworkSceneManager { get; }
    public ServerObjectManager ServerObjectManager { get; }
    public ClientObjectManager ClientObjectManager { get; }
    public bool IsServer => Server != null && Server.Active;
    public bool IsClient => Client != null && Client.Active;
    public bool IsHost => IsServer && IsClient;

    public NetworkContext(NetworkManager networkManager)
    {
        NetworkManager = networkManager;
        Server = networkManager.Server;
        Client = networkManager.Client;
        NetworkSceneManager = networkManager.NetworkSceneManager;
        ServerObjectManager = networkManager.ServerObjectManager;
        ClientObjectManager = networkManager.ClientObjectManager;
    }
}