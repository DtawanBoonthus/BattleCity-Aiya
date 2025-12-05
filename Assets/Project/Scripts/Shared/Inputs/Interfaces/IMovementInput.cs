using UnityEngine;

namespace BC.Shared.Inputs;

public interface IMovementInput
{
    Vector2 Move { get; }
}