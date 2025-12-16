using R3;
using VContainer;

namespace BC.UI;

public class GameEndViewModel : IGameEndViewModel
{
    [Inject] private readonly GameEndViewModelRouter router = null!;

    public Observable<string> OnGameEnd => router.OnGameEnd;
    public Observable<Unit> OnRestartGame => router.OnRestartGame;
}