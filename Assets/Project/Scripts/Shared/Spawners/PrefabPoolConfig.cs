using System.Collections.Generic;
using UnityEngine;

namespace BC.Shared.Spawners
{
    [CreateAssetMenu(fileName = "Prefab Pool Config", menuName = "BC/Prefab Pool Config")]
    public class PrefabPoolConfig : ScriptableObject
    {
        [SerializeField] private List<PoolItem> items = new();

        public IReadOnlyList<PoolItem> Items => items;
    }
}