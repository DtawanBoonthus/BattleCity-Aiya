using BC.Gameplay;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.UI;

[Routes]
public partial class GameEndViewModelRouter
{
    private readonly Subject<string> gameEnd = new();
    private readonly Subject<Unit> restartGame = new();
    public Observable<string> OnGameEnd => gameEnd;
    public Observable<Unit> OnRestartGame => restartGame;

    [Route]
    private UniTask OnResultEndGameCommand(ResultEndGameCommand command)
    {
        gameEnd.OnNext(command.Status);
        return UniTask.CompletedTask;
    }

    [Route]
    private UniTask OnRestartGameCommand(RestartGameCommand command)
    {
        restartGame.OnNext(Unit.Default);
        return UniTask.CompletedTask;
    }
}