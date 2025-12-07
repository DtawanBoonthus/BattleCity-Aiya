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
            builder.RegisterInstance(networkManager).AsSelf();
        }
    }
}