using R3;

namespace BC.UI;

public interface IGameStartCountdownViewModel
{
    ReadOnlyReactiveProperty<int> SecondsLeft { get; }
    Observable<Unit> OnEndCooldown { get; }
}