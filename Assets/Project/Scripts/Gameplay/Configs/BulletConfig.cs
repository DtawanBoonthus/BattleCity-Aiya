using UnityEngine;

namespace BC.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "BC/Bullet Config")]
    public class BulletConfig : ScriptableObject, IBulletConfig
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 1;
        [SerializeField] private int staggerTime = 1;
        [SerializeField] private LayerMask hitMask = 1 << 8;

        public float Speed => speed;
        public int Damage => damage;
        public int StaggerTime => staggerTime;
        public LayerMask HitMask => hitMask;
    }
}