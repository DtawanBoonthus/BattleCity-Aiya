using BC.Core.Networks;
using BC.Shared.Networks;
using Mirage;
using Mirage.Sockets.Udp;
using UnityEngine;

namespace BC.Bootstrap
{
    public class GlobalLifetimeScope : LifetimeScope
    {
        [SerializeField] private NetworkManager networkManager = null!;
        [SerializeField] private UdpSocketFactory udpSocketFactory = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            new InputInstaller().Install(builder);
            builder.RegisterInstance(networkManager).AsSelf();
            builder.RegisterInstance(udpSocketFactory).AsSelf();

            builder.Register<MirageNetworkConfig>(Lifetime.Singleton).As<INetworkConfig, IStartable>();
            builder.Register<MirageNetworkContext>(Lifetime.Singleton).As<INetworkContext>();
            builder.Register<MirageNetworkLifecycle>(Lifetime.Singleton).As<INetworkLifecycle>();
        }
    }
}