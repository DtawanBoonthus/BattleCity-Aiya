using System.Collections.Generic;
using UnityEngine;

namespace BC.Shared.Spawners
{
    [CreateAssetMenu(fileName = "PrefabPoolConfig", menuName = "BC/Prefab Pool Config")]
    public class PrefabPoolConfig : ScriptableObject
    {
        [SerializeField] private List<PoolItem> items = new();

        public IReadOnlyList<PoolItem> Items => items;
    }
}