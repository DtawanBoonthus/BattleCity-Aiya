using BC.Shared.Networks;
using Mirage;
using VContainer;

namespace BC.Core.Networks;

public class MirageNetworkLifecycle : INetworkLifecycle
{
    [Inject] private readonly NetworkManager networkManager = null!;

    public void StartServer()
    {
        networkManager.Server.StartServer();
    }

    public void StartClient(string address)
    {
        networkManager.Client.Connect(address);
    }

    public void StartHost()
    {
        networkManager.Server.StartServer(networkManager.Client);
    }

    public void StopServer()
    {
        networkManager.Server.Stop();
    }

    public void StopClient()
    {
        networkManager.Client.Disconnect();
    }
}