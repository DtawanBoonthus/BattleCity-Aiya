using System;
using BC.Shared.Networks;
using Cysharp.Threading.Tasks;
using Mirage;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Networks
{
    public class MirageNetworkLifecycle : INetworkLifecycle, IStartable, IDisposable
    {
        [Inject] private readonly NetworkManager networkManager = null!;

        private UniTaskCompletionSource<ConnectRoomStatus>? connectTcs;
        private bool waitingServer;
        private bool waitingClient;

        private const int TIMEOUT_MS = 5000;

        public void Start()
        {
            networkManager.Server.Started.AddListener(OnServerStarted);
            networkManager.Client.Connected.AddListener(OnClientConnected);
            networkManager.Client.Disconnected.AddListener(OnClientDisconnected);
        }

        public void Dispose()
        {
            networkManager.Server.Started.RemoveListener(OnServerStarted);
            networkManager.Client.Connected.RemoveListener(OnClientConnected);
            networkManager.Client.Disconnected.RemoveListener(OnClientDisconnected);
        }

        public async UniTask<ConnectRoomStatus> StartServer()
        {
            connectTcs = new UniTaskCompletionSource<ConnectRoomStatus>();

            waitingServer = true;
            waitingClient = false;

            networkManager.Server.StartServer();

            return await WaitOrTimeout();
        }

        public async UniTask<ConnectRoomStatus> StartClient(string address)
        {
            connectTcs = new UniTaskCompletionSource<ConnectRoomStatus>();

            waitingServer = false;
            waitingClient = true;

            networkManager.Client.Connect(address);

            return await WaitOrTimeout();
        }

        public async UniTask<ConnectRoomStatus> StartHost()
        {
            connectTcs = new UniTaskCompletionSource<ConnectRoomStatus>();

            waitingServer = true;
            waitingClient = true;

            networkManager.Server.StartServer(networkManager.Client);

            return await WaitOrTimeout();
        }

        public void StopServer()
        {
            networkManager.Server.Stop();
        }

        public void StopClient()
        {
            networkManager.Client.Disconnect();
        }

        private void OnServerStarted()
        {
            if (waitingServer)
            {
                waitingServer = false;
                TryFinishSuccess();
            }
        }

        private void OnClientConnected(INetworkPlayer player)
        {
            if (waitingClient)
            {
                waitingClient = false;
                TryFinishSuccess();
            }
        }

        private void OnClientDisconnected(ClientStoppedReason reason)
        {
            connectTcs?.TrySetResult(ConnectRoomStatus.Failed);
        }

        private void TryFinishSuccess()
        {
            if (!waitingServer && !waitingClient)
            {
                connectTcs?.TrySetResult(ConnectRoomStatus.Success);
            }
        }

        private async UniTask<ConnectRoomStatus> WaitOrTimeout()
        {
            var connectTask = connectTcs!.Task.AsUniTask();
            var timeoutTask = UniTask.Delay(TIMEOUT_MS);

            int index = await UniTask.WhenAny(connectTask, timeoutTask);

            if (index == 0)
            {
                return await connectTcs.Task;
            }

            connectTcs.TrySetResult(ConnectRoomStatus.Timeout);
            return ConnectRoomStatus.Timeout;
        }
    }
}