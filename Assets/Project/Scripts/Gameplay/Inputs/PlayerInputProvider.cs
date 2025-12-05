using System;
using BC.Shared.Inputs;
using UnityEngine;
using VContainer.Unity;

namespace BC.Gameplay.Inputs;

public partial class PlayerInputProvider : IInputProvider, IStartable, IDisposable
{
    public Vector2 Move { get; }
    public bool IsAttack { get; }
    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}