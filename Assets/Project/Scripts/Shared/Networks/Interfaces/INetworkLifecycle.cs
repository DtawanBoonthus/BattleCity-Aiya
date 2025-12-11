using Cysharp.Threading.Tasks;

namespace BC.Shared.Networks;

public interface INetworkLifecycle
{
    UniTask<ConnectRoomStatus> StartServer();
    UniTask<ConnectRoomStatus> StartClient(string address);
    UniTask<ConnectRoomStatus> StartHost();

    void StopServer();
    void StopClient();
}