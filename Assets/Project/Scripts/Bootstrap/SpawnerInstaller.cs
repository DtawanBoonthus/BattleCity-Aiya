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
        builder.Register<MirageSpawner>(Lifetime.Singleton).As<ISpawnService<MirageNet>>();

        builder.RegisterBuildCallback(container =>
        {
            var spawnService = container.Resolve<ISpawnService<MirageNet>>();
            spawnService.RegisterPools(config.Items);
        });
    }
}