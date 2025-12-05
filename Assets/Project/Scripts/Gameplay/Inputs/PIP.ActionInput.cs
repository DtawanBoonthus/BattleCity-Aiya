namespace BC.Gameplay.Inputs;

public partial class PlayerInputProvider
{
    public bool IsAttack => Input.Player.Attack.triggered;
}