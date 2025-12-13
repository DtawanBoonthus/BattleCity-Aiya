using BC.Shared.GameSessions;
using VitalRouter;

namespace BC.Gameplay;

public readonly record struct GameModeCommand(GameMode GameMode) : ICommand;