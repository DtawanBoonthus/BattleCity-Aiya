using System.Collections.Generic;
using UnityEngine;

namespace BC.Gameplay.Configs;

public interface IGameplayConfig
{
    int StartGameDurationSecond { get; }
    IReadOnlyList<Sprite> TanksSprites { get; }
    GameObject TankPrefab { get; }
    GameObject BulletPrefab { get; }
    GameObject GameStartCountdownPrefab { get; }
}