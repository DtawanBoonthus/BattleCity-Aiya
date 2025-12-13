using VitalRouter;

namespace BC.Gameplay;

public readonly record struct GameStartCountdownTickCommand(int SecondsLeft) : ICommand;