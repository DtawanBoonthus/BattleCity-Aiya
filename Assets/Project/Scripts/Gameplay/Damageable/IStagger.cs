namespace BC.Gameplay.Damageable;

public interface IStagger
{
    void Stagger(int staggerTime);
    bool IsStaggered { get; }
}