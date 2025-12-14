using System;
using BC.Core.Inputs;
using BC.Gameplay.Inputs;
using BC.Shared.Inputs;

namespace BC.Bootstrap;

public class InputInstaller : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.Register<PlayerInputAction>(Lifetime.Singleton);

        builder.Register<IInputProvider, InputRouter>(Lifetime.Singleton).AsSelf();

        builder.Register<PlayerInputProvider>(Lifetime.Singleton)
            .AsSelf()
            .As<IStartable, IDisposable>();

        builder.RegisterBuildCallback(container =>
        {
            var router = container.Resolve<InputRouter>();
            var provider = container.Resolve<PlayerInputProvider>();
            router.SetProvider(provider);
        });
    }
}