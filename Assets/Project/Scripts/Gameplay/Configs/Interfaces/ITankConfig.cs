namespace BC.Gameplay.Configs;

public interface ITankConfig
{
    float StartMoveSpeed { get; }
    int MaxHealth { get; }
    float FireCooldown { get; }
    float IFrameTime { get; }
}