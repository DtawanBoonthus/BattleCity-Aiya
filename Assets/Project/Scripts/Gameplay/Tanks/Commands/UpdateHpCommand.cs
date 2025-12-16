using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct UpdateHpCommand(uint ID, int Hp) : ICommand;