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
            new NetworkInstaller(networkManager, udpSocketFactory).Install(builder);
        }
    }
}