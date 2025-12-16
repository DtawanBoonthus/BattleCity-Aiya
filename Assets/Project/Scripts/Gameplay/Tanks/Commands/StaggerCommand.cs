using VitalRouter;

namespace BC.Gameplay.Tanks;

public readonly record struct StaggerCommand(uint NetId, int StaggerTime) : ICommand;