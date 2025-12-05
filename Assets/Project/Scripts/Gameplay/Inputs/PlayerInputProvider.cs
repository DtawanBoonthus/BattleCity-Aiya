using System;
using BC.Shared.Inputs;
using VContainer;
using VContainer.Unity;

namespace BC.Gameplay.Inputs;

public partial class PlayerInputProvider : IInputProvider, IStartable, IDisposable
{
    [Inject] private PlayerInputAction Input { get; set; } = null!;

    public void Start()
    {
        Input.Player.Enable();
        BindMovementInput();
    }

    public void Dispose()
    {
        Input.Player.Disable();
        UnbindMovementInput();
    }

    private partial void BindMovementInput();
    private partial void UnbindMovementInput();
}