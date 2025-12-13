using System.Collections.Generic;
using UnityEngine;

namespace BC.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "BC/Gameplay Config")]
    public class GameplayConfig : ScriptableObject, IGameplayConfig
    {
        [SerializeField] private int startGameDurationSecond = 3;

        [Space] [SerializeField] private List<Sprite> tanksSprites = new();
        [SerializeField] private GameObject tankPrefab = null!;
        [SerializeField] private GameObject bulletPrefab = null!;
        [SerializeField] private GameObject gameStartCountdownPrefab = null!;

        public int StartGameDurationSecond => startGameDurationSecond;
        public IReadOnlyList<Sprite> TanksSprites => tanksSprites;
        public GameObject TankPrefab => tankPrefab;
        public GameObject BulletPrefab => bulletPrefab;
        public GameObject GameStartCountdownPrefab => gameStartCountdownPrefab;
    }
}