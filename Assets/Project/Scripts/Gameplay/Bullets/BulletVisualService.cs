using System.Collections.Generic;

namespace BC.Gameplay;

public class BulletVisualService
{
    private readonly Dictionary<uint, BulletVisual> visuals = new();

    public void AddBulletVisual(uint id, BulletVisual visual)
    {
        visuals.Add(id, visual);
    }

    public void DespawnBulletVisual(uint id)
    {
        if (!visuals.TryGetValue(id, out var bulletVisual))
        {
            return;
        }

        bulletVisual.Destroy();
        visuals.Remove(id);
    }
}