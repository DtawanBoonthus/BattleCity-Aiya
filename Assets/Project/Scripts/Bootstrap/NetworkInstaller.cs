using System;
using BC.Core.Networks;
using BC.Shared.Networks;
using Mirage;
using Mirage.Sockets.Udp;

namespace BC.Bootstrap;

public class NetworkInstaller : IInstaller
{
    private readonly NetworkManager networkManager;
    private readonly UdpSocketFactory udpSocketFactory;

    public NetworkInstaller(NetworkManager networkManager, UdpSocketFactory udpSocketFactory)
    {
        this.networkManager = networkManager;
        this.udpSocketFactory = udpSocketFactory;
    }

    public void Install(IContainerBuilder builder)
    {
        builder.RegisterInstance(networkManager).AsSelf();
        builder.RegisterInstance(udpSocketFactory).AsSelf();

        builder.Register<MirageNetworkConfig>(Lifetime.Singleton).As<INetworkConfig>();
        builder.Register<MirageNetworkContext>(Lifetime.Singleton).As<INetworkContext>();
        builder.Register<MirageNetworkLifecycle>(Lifetime.Singleton).As<INetworkLifecycle, IStartable, IDisposable>();
        builder.Register<MirageNetworkRoomService>(Lifetime.Singleton).As<INetworkRoomService, IStartable, IDisposable>();
    }
}