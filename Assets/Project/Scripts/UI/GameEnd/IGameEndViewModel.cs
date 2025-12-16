using R3;

namespace BC.UI;

public interface IGameEndViewModel
{
    Observable<string> OnGameEnd { get; }
    Observable<Unit> OnRestartGame { get; }
}