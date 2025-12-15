using BC.Gameplay;
using Cysharp.Threading.Tasks;
using R3;
using VitalRouter;

namespace BC.UI;

[Routes]
public partial class GameModeViewModelRouter
{
    private readonly Subject<Unit> connectSuccess = new();
    public Observable<Unit> OnConnectSuccess => connectSuccess;

    [Route]
    private UniTask OnConnectGameSuccess(ConnectGameSuccess command)
    {
        connectSuccess.OnNext(Unit.Default);
        return UniTask.CompletedTask;
    }
}