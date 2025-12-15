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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                var spawnService = container.Resolve<ISpawnService<MirageNet>>();
                spawnService.RegisterNetworkPrefab(new[] { gameplayConfig.GameStartCountdownPrefab });
            });

            builder.RegisterInstance(gameplayConfig).As<IGameplayConfig>();
            builder.RegisterVitalRouter(routing =>
            {
                routing.Map<GameController>();
                routing.Map<MatchControllerRouter>();
                routing.Map<GameModeViewModelRouter>();
                routing.Map<GameStartCountdownViewModelRouter>();
            });

            builder.Register<MatchController>(Lifetime.Scoped).As<IStartable, IDisposable>();
            builder.Register<IGameModeViewModel, GameModeViewModel>(Lifetime.Scoped);
            builder.Register<IGameStartCountdownViewModel, GameStartCountdownViewModel>(Lifetime.Scoped).As<IStartable, IDisposable>();
        }
    }
}