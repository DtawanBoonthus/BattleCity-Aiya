using UnityEngine;

namespace BC.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "TankConfig", menuName = "BC/Tank Config")]
    public class TankConfig : ScriptableObject, ITankConfig
    {
        [SerializeField] private float startSpeed = 10f;
        
        public float StartSpeed => startSpeed;
    }
}