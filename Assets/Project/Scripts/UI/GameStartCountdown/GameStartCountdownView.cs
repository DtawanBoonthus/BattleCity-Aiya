using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace BC.UI
{
    public class GameStartCountdownView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownTMP = null!;

        private IGameStartCountdownViewModel gameStartCountdownViewModel = null!;

        private CompositeDisposable? disposables;
        private CompositeDisposable? destroyDisposables;

        [Inject]
        private void Construct(IGameStartCountdownViewModel viewModel)
        {
            destroyDisposables ??= new CompositeDisposable();
            gameStartCountdownViewModel = viewModel;
            gameStartCountdownViewModel.SecondsLeft.Subscribe(UpdateCountdownDisplay).AddTo(destroyDisposables);
        }

        private void OnEnable()
        {
            disposables ??= new CompositeDisposable();
            gameStartCountdownViewModel.OnEndCooldown.Subscribe(HandleCooldownEnd).AddTo(disposables);
        }

        private void OnDisable()
        {
            disposables?.Dispose();
            disposables = null;
        }

        private void OnDestroy()
        {
            destroyDisposables?.Dispose();
            destroyDisposables = null;
        }

        private void UpdateCountdownDisplay(int secondsLeft)
        {
            if (!gameObject.activeSelf && secondsLeft > 0)
            {
                gameObject.SetActive(true);
            }

            countdownTMP.text = secondsLeft.ToString();
        }

        private void HandleCooldownEnd(Unit _)
        {
            gameObject.SetActive(false);
        }
    }
}