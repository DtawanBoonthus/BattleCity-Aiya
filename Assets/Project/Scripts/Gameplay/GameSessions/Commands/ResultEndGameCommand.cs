using VitalRouter;

namespace BC.Gameplay;

public readonly record struct ResultEndGameCommand(string Status) : ICommand;