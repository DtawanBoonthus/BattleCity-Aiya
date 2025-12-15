using BC.Gameplay;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.UI;

[Routes]
public partial class GameStartCountdownViewModelRouter
{
    private readonly ReactiveProperty<int> secondsLeft = new(0);

    public ReadOnlyReactiveProperty<int> SecondsLeft => secondsLeft;

    [Route]
    private UniTask OnConnectGameSuccess(GameStartCountdownTickCommand command)
    {
        secondsLeft.Value = command.SecondsLeft;
        return UniTask.CompletedTask;
    }
}