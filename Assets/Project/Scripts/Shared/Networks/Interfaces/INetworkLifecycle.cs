namespace BC.Shared.Networks;

public interface INetworkLifecycle
{
    void StartServer();
    void StartClient(string address);
    void StartHost();

    void StopServer();
    void StopClient();
}