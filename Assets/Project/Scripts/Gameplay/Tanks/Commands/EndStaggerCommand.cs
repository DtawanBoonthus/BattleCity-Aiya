using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct EndStaggerCommand(uint NetId) : ICommand;