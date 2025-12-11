using System;
using BC.Gameplay.Matchs;

namespace BC.Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MatchController>(Lifetime.Scoped).As<IAsyncStartable, IDisposable>();
        }
    }
}