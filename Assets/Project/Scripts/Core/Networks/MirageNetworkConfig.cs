using BC.Shared.Networks;
using Mirage;
using Mirage.Sockets.Udp;
using VContainer;

namespace BC.Core.Networks;

public class MirageNetworkConfig : INetworkConfig
{
    [Inject] private readonly NetworkManager networkManager = null!;
    [Inject] private readonly UdpSocketFactory udpSocket = null!;

    public string Address => udpSocket.Address;
    public int Port => udpSocket.Port;
    public int MaxConnections => networkManager.Server.MaxConnections;

    public void SetAddress(string address)
    {
        udpSocket.Address = address;
    }

    public void SetPort(int port)
    {
        udpSocket.Port = (ushort)port;
    }

    public void SetMaxConnections(int maxConnections)
    {
        networkManager.Server.MaxConnections = maxConnections;
    }
}