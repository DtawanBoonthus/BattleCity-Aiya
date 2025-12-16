using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct IFrameCommand(uint NetId, float IFrameTime) : ICommand;