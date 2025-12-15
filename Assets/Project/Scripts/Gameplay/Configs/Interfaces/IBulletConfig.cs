using UnityEngine;

namespace BC.Gameplay.Configs;

public interface IBulletConfig
{
    float Speed { get; }
    int Damage { get; }
    int StaggerTime { get; }
    LayerMask HitMask { get; }
}