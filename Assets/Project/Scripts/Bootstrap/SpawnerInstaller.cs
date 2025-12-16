using BC.Core.Spawners;
using BC.Shared.Spawners;

namespace BC.Bootstrap;

public class SpawnerInstaller : IInstaller
{
    private readonly PrefabPoolConfig config;

    public SpawnerInstaller(PrefabPoolConfig config)
    {
        this.config = config;
    }

    public void Install(IContainerBuilder builder)
    {
        builder.Register<ISpawnService<MirageNet>, MirageSpawner>(Lifetime.Scoped);
        builder.Register<ISpawnService<Normal>, NormalSpawner>(Lifetime.Scoped);

        builder.RegisterBuildCallback(container =>
        {
            var mirageSpawnService = container.Resolve<ISpawnService<MirageNet>>();
            var normalSpawnService = container.Resolve<ISpawnService<Normal>>();
            mirageSpawnService.RegisterPools(config.Items);
            normalSpawnService.RegisterPools(config.Items);
        });
    }
}