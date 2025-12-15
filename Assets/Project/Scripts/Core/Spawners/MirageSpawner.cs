using System.Collections.Generic;
using BC.Shared.Spawners;
using Mirage;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Spawners;

public class MirageSpawner : ISpawnService<MirageNet>
{
    [Inject] private readonly NetworkManager networkManager = null!;
    [Inject] private readonly IObjectResolver resolver = null!;

    private readonly HashSet<int> registered = new();
    private readonly Dictionary<int, PrefabPool> pools = new();
    private readonly Transform poolRoot;

    public MirageSpawner()
    {
        var rootObj = new GameObject("NetworkPoolRoot");
        Object.DontDestroyOnLoad(rootObj);
        poolRoot = rootObj.transform;
    }

    public void RegisterPools(IEnumerable<PoolItem> items)
    {
        foreach (var item in items)
        {
            if (item.Prefab == null)
            {
                continue;
            }

            var networkIdentity = item.Prefab.GetComponent<NetworkIdentity>();
            var hash = networkIdentity.PrefabHash;

            if (!registered.Contains(hash))
            {
                networkManager.ClientObjectManager.RegisterSpawnHandler(
                    networkIdentity,
                    spawnMessage => SpawnHandler(hash, spawnMessage),
                    identity => UnSpawnHandler(hash, identity)
                );

                registered.Add(hash);
            }

            pools[hash] = new PrefabPool(
                item.Prefab,
                item.StartSize,
                item.MaxSize,
                item.Unlimited,
                poolRoot
            );
        }
    }

    public void RegisterNetworkPrefab(IEnumerable<GameObject> items)
    {
        foreach (var item in items)
        {
            if (item == null)
            {
                Debug.LogWarning($"Network prefab is null. {nameof(item)}");
                continue;
            }

            var networkIdentity = item.GetComponent<NetworkIdentity>();
            var hash = networkIdentity.PrefabHash;

            if (registered.Contains(hash))
            {
                return;
            }

            networkManager.ClientObjectManager.RegisterSpawnHandler(
                networkIdentity,
                spawnMessage => SpawnHandler(hash, spawnMessage),
                identity => UnSpawnHandler(hash, identity)
            );

            registered.Add(hash);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var networkIdentity = prefab.GetComponent<NetworkIdentity>();
        var hash = networkIdentity.PrefabHash;

        GameObject gameObject;

        if (pools.TryGetValue(hash, out var pool))
        {
            gameObject = pool.Spawn(position, rotation);
            resolver.InjectGameObject(gameObject);
        }
        else
        {
            gameObject = Object.Instantiate(prefab, position, rotation);
            resolver.InjectGameObject(gameObject);
        }

        networkManager.ServerObjectManager.Spawn(gameObject.GetComponent<NetworkIdentity>());
        return gameObject;
    }

    public void Despawn(GameObject instance)
    {
        var networkIdentity = instance.GetComponent<NetworkIdentity>();
        var hash = networkIdentity.PrefabHash;

        networkManager.ServerObjectManager.Destroy(networkIdentity, destroyServerObject: false);

        if (pools.TryGetValue(hash, out var pool))
        {
            pool.Despawn(instance);
        }
        else
        {
            Object.Destroy(instance);
        }
    }

    private NetworkIdentity SpawnHandler(int hash, SpawnMessage spawnMessage)
    {
        var spawnPosition = spawnMessage.SpawnValues.Position ?? Vector3.zero;
        var spawnRotation = spawnMessage.SpawnValues.Rotation ?? Quaternion.identity;

        if (pools.TryGetValue(hash, out var pool))
        {
            var gameObject = pool.Spawn(spawnPosition, spawnRotation);
            resolver.InjectGameObject(gameObject);
            return gameObject.GetComponent<NetworkIdentity>();
        }

        var prefab = networkManager.ClientObjectManager.GetSpawnHandler(hash).Prefab;
        var fallback = Object.Instantiate(prefab, spawnPosition, spawnRotation);
        resolver.InjectGameObject(fallback.gameObject);
        return fallback;
    }

    private void UnSpawnHandler(int hash, NetworkIdentity identity)
    {
        if (pools.TryGetValue(hash, out var pool))
        {
            pool.Despawn(identity.gameObject);
        }
        else
        {
            Object.Destroy(identity.gameObject);
        }
    }
}