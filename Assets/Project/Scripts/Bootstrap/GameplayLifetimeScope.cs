using BC.Core.Networks;
using BC.Shared.Networks;

namespace BC.Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MirageNetworkContext>(Lifetime.Scoped).As<INetworkContext>();
        }
    }
}