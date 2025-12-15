using System;
using BC.Gameplay;
using BC.UI;

namespace BC.Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<MatchController>(Lifetime.Scoped).As<IStartable, IDisposable>();
            builder.Register<IGameModeViewModel, GameModeViewModel>(Lifetime.Scoped);
            builder.Register<IGameStartCountdownViewModel, GameStartCountdownViewModel>(Lifetime.Scoped).As<IStartable, IDisposable>();
        }
    }
}