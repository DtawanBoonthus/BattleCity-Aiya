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

        builder.Register<INetworkConfig, MirageNetworkConfig>(Lifetime.Singleton);
        builder.Register<INetworkContext, MirageNetworkContext>(Lifetime.Singleton);
        builder.Register<INetworkLifecycle, MirageNetworkLifecycle>(Lifetime.Singleton).As<IStartable, IDisposable>();
        builder.Register<INetworkRoomService, MirageNetworkRoomService>(Lifetime.Singleton).As<IStartable, IDisposable>();
    }
}