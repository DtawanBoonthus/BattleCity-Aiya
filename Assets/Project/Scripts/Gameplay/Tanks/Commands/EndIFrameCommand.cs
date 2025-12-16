using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct EndIFrameCommand(uint NetId) : ICommand;