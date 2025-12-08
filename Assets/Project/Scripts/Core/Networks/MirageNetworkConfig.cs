using BC.Shared.Networks;
using Mirage;
using Mirage.Sockets.Udp;
using R3;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Networks;

public class MirageNetworkConfig : INetworkConfig, IStartable
{
    [Inject] private readonly NetworkManager networkManager = null!;
    [Inject] private readonly UdpSocketFactory udpSocket = null!;

    private readonly ReactiveProperty<string> address = new();
    private readonly ReactiveProperty<int> port = new();
    private readonly ReactiveProperty<int> maxConnections = new();

    public ReadOnlyReactiveProperty<string> Address => address;
    public ReadOnlyReactiveProperty<int> Port => port;
    public ReadOnlyReactiveProperty<int> MaxConnections => maxConnections;

    public void Start()
    {
        SetAddress(udpSocket.Address);
        SetPort(udpSocket.Port);
        SetMaxConnections(networkManager.Server.MaxConnections);
    }

    public void SetAddress(string address)
    {
        this.address.Value = address;
        udpSocket.Address = address;
    }

    public void SetPort(int port)
    {
        this.port.Value = port;
        udpSocket.Port = (ushort)port;
    }

    public void SetMaxConnections(int maxConnections)
    {
        this.maxConnections.Value = maxConnections;
        networkManager.Server.MaxConnections = maxConnections;
    }
}