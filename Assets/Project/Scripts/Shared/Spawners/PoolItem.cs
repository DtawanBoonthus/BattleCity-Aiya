using System;
using UnityEngine;

namespace BC.Shared.Spawners;

[Serializable]
public class PoolItem
{
    [field: SerializeField] public GameObject Prefab { get; private set; } = null!;
    [field: SerializeField] public int StartSize { get; private set; }
    [field: SerializeField] public int MaxSize { get; private set; }
    [field: SerializeField] public bool Unlimited { get; private set; }
}