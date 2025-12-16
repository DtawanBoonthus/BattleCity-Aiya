using System.Collections.Generic;
using BC.Shared.Spawners;
using Mirage;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BC.Core.Spawners;

public class NormalSpawner : ISpawnService<Normal>
{
    [Inject] private readonly IObjectResolver resolver = null!;

    private readonly Dictionary<GameObject, PrefabPool> pools = new();
    private readonly Transform poolRoot;

    public NormalSpawner()
    {
        var rootObj = new GameObject("NormalPoolRoot");
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

            if (pools.ContainsKey(item.Prefab))
            {
                continue;
            }

            pools[item.Prefab] = new PrefabPool(
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
    }

    public GameObject Spawn
    (
        GameObject prefab,
        Vector3 position,
        Quaternion rotation,
        INetworkPlayer? owner = null
    )
    {
        var instance = pools.TryGetValue(prefab, out var pool)
            ? pool.Spawn(position, rotation)
            : Object.Instantiate(prefab, position, rotation);

        resolver.InjectGameObject(instance);
        return instance;
    }

    public void Despawn(GameObject instance)
    {
        foreach (var kv in pools)
        {
            if (kv.Value.Contains(instance))
            {
                kv.Value.Despawn(instance);
                return;
            }
        }

        Object.Destroy(instance);
    }
}