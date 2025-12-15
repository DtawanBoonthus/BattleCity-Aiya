using BC.Gameplay;
using BC.Gameplay.Configs;
using BC.Shared.Spawners;
using BC.UI;
using Mirage;
using Mirage.Sockets.Udp;
using UnityEngine;
using VitalRouter.VContainer;

namespace BC.Bootstrap
{
    public class GlobalLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayConfig gameplayConfig = null!;
        [SerializeField] private NetworkManager networkManager = null!;
        [SerializeField] private UdpSocketFactory udpSocketFactory = null!;
        [SerializeField] private PrefabPoolConfig prefabPoolConfig = null!;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(container =>
            {
                var spawnService = container.Resolve<ISpawnService<MirageNet>>();
                spawnService.RegisterNetworkPrefab(gameplayConfig.NetworkPrefabs);
            });

            builder.RegisterInstance(gameplayConfig).As<IGameplayConfig>();

            builder.RegisterVitalRouter(routing =>
            {
                routing.Map<GameControllerRouter>();
                routing.Map<MatchControllerRouter>();
                routing.Map<GameModeViewModelRouter>();
                routing.Map<GameStartCountdownViewModelRouter>();
            });

            new InputInstaller().Install(builder);
            new NetworkInstaller(networkManager, udpSocketFactory).Install(builder);
            new SpawnerInstaller(prefabPoolConfig).Install(builder);
        }
    }
}