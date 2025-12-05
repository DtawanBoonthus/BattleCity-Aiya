using UnityEngine;
using UnityEngine.InputSystem;

namespace BC.Gameplay.Inputs;

public partial class PlayerInputProvider
{
    public Vector2 Move { get; private set; }

    private partial void BindMovementInput()
    {
        Input.Player.Move.performed += OnMove;
        Input.Player.Move.canceled += OnMove;
    }

    private partial void UnbindMovementInput()
    {
        Input.Player.Move.performed -= OnMove;
        Input.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext callback)
    {
        Move = callback.ReadValue<Vector2>();
    }
}