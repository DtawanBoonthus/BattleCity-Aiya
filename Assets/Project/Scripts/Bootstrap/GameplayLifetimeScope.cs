using System;
using BC.Gameplay;
using BC.Gameplay.Configs;
using BC.Shared.Spawners;
using BC.UI;
using UnityEngine;
using VitalRouter.VContainer;

namespace BC.Bootstrap
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayConfig gameplayConfig = null!;
        [SerializeField] private SpawnPosition spawnPosition = null!;
        [SerializeField] private PrefabPoolConfig prefabPoolConfig = null!;
        [SerializeField] private TankConfig tankConfig = null!;
        [SerializeField] private BulletConfig bulletConfig = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BulletVisualService>(Lifetime.Scoped);
            builder.RegisterInstance(tankConfig).As<ITankConfig>().AsSelf();
            builder.RegisterInstance(bulletConfig).As<IBulletConfig>().AsSelf();

            builder.RegisterBuildCallback(container =>
            {
                var spawnService = container.Resolve<ISpawnService<MirageNet>>();
                spawnService.RegisterNetworkPrefab(gameplayConfig.NetworkPrefabs);
            });

            builder.RegisterInstance(gameplayConfig).As<IGameplayConfig>();

            new SpawnerInstaller(prefabPoolConfig).Install(builder);
            builder.RegisterInstance(spawnPosition).AsSelf();

            builder.RegisterVitalRouter(routing =>
            {
                routing.Map<GameControllerRouter>();
                routing.Map<MatchControllerRouter>();
                routing.Map<GameModeViewModelRouter>();
                routing.Map<GameStartCountdownViewModelRouter>();
                routing.Map<TankWorldUIRouter>();
                routing.Map<GameEndViewModelRouter>();
            });

            builder.Register<MatchController>(Lifetime.Scoped).As<IStartable, IDisposable>();
            builder.Register<IGameModeViewModel, GameModeViewModel>(Lifetime.Scoped);
            builder.Register<IGameStartCountdownViewModel, GameStartCountdownViewModel>(Lifetime.Scoped).As<IStartable, IDisposable>();
            builder.Register<IGameEndViewModel, GameEndViewModel>(Lifetime.Scoped);
        }
    }
}