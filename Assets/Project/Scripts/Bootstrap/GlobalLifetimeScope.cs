using BC.Shared.Networks;
using Mirage;
using UnityEngine;

namespace BC.Bootstrap
{
    public class GlobalLifetimeScope : LifetimeScope
    {
        [SerializeField] private NetworkManager networkManager = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            new InputInstaller().Install(builder);

            var networkContext = new NetworkContext(networkManager);
            builder.RegisterInstance<INetworkContext>(networkContext);
        }
    }
}