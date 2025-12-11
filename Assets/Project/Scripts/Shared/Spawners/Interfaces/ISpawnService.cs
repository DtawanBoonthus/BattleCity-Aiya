using System.Collections.Generic;
using UnityEngine;

namespace BC.Shared.Spawners;

public interface ISpawnService<T> where T : ISpawnerTag
{
    void RegisterPools(IEnumerable<PoolItem> items);
    GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
    void Despawn(GameObject instance);
}