using BC.Shared.Inputs;
using UnityEngine;

namespace BC.Core.Inputs;

public class InputRouter : IInputProvider
{
    private IInputProvider? current;

    public Vector2 Move => current?.Move ?? Vector2.zero;
    public bool IsAttack => current?.IsAttack ?? false;

    public void SetProvider(IInputProvider provider) => current = provider;
}