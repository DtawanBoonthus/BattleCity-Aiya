using BC.Gameplay;
using BC.Shared.GameSessions;
using Cysharp.Threading.Tasks;
using R3;
using VContainer;
using VitalRouter;

namespace BC.UI;

public class GameModeViewModel : IGameModeViewModel
{
    [Inject] private readonly ICommandPublisher publisher = null!;
    [Inject] private readonly GameModeViewModelRouter gameModeViewModelRouter = null!;

    public Observable<Unit> OnConnectSuccess => gameModeViewModelRouter.OnConnectSuccess;

    public async UniTask ConnectGameAsync(GameMode mode)
    {
        await publisher.PublishAsync(new GameModeCommand(mode));
    }
}