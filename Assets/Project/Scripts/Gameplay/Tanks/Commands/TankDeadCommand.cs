using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct TankDeadCommand(uint PlayerWin, uint PlayerLose) : ICommand;