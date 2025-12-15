using UnityEngine;

namespace BC.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "BC/Tank Config")]
    public class TankConfig : ScriptableObject, ITankConfig
    {
        [SerializeField] private float startMoveSpeed = 10f;
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float fireCooldown = 0.2f;
        [SerializeField] private float iframeTime = 0.5f;

        public float StartMoveSpeed => startMoveSpeed;
        public int MaxHealth => maxHealth;
        public float FireCooldown => fireCooldown;
        public float IFrameTime => iframeTime;
    }
}