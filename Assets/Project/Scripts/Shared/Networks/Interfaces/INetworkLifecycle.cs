using Cysharp.Threading.Tasks;

namespace BC.Shared.Networks;

public interface INetworkLifecycle
{
    UniTask<ConnectRoomStatus> StartServerAsync();
    UniTask<ConnectRoomStatus> StartClientAsync(string address);
    UniTask<ConnectRoomStatus> StartHostAsync();

    void StopServer();
    void StopClient();
}