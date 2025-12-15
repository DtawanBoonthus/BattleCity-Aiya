using System.Collections.Generic;
using Mirage;
using UnityEngine;

namespace BC.Shared.Spawners;

public interface ISpawnService<T> where T : ISpawnerTag
{
    void RegisterPools(IEnumerable<PoolItem> items);
    void RegisterNetworkPrefab(IEnumerable<GameObject> items);
    GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, INetworkPlayer? owner = null);
    void Despawn(GameObject instance);
}