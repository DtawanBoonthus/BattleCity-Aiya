using System;
using R3;
using VContainer;
using VContainer.Unity;

namespace BC.UI;

public class GameStartCountdownViewModel : IGameStartCountdownViewModel, IStartable, IDisposable
{
    [Inject] private readonly GameStartCountdownViewModelRouter gameStartCountdownViewModelRouter = null!;

    private readonly Subject<Unit> endCooldown = new();

    public Observable<Unit> OnEndCooldown => endCooldown;
    public ReadOnlyReactiveProperty<int> SecondsLeft => gameStartCountdownViewModelRouter.SecondsLeft;

    private CompositeDisposable? disposables;

    public void Start()
    {
        disposables ??= new CompositeDisposable();
        gameStartCountdownViewModelRouter.SecondsLeft.Subscribe(UpdateSecondsLeft).AddTo(disposables);
    }

    public void Dispose()
    {
        disposables?.Dispose();
    }

    private void UpdateSecondsLeft(int secondsLeft)
    {
        if (secondsLeft <= 0)
        {
            endCooldown.OnNext(Unit.Default);
        }
    }
}