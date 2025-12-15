using System.Collections.Generic;
using UnityEngine;

namespace BC.Gameplay
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawnPositions = new();
        public IReadOnlyList<Transform> SpawnPositions => spawnPositions;
    }
}