using BC.Shared.GameSessions;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.Gameplay;

[Routes]
public partial class MatchControllerRouter
{
    private readonly Subject<GameMode> gameModeCommand = new();
    public Observable<GameMode> OnGameModeCommand => gameModeCommand;

    [Route]
    private UniTask OnGameModeCommandAsync(GameModeCommand command)
    {
        gameModeCommand.OnNext(command.GameMode);
        return UniTask.CompletedTask;
    }
}