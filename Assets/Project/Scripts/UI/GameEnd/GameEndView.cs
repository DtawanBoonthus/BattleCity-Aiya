using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace BC.UI
{
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameEndStatusTMP = null!;

        private IGameEndViewModel gameEndViewModel = null!;
        private CompositeDisposable? destroyDisposables;

        [Inject]
        private void Construct(IGameEndViewModel viewModel)
        {
            destroyDisposables ??= new CompositeDisposable();
            gameEndViewModel = viewModel;
            gameEndViewModel.OnGameEnd.Subscribe(SetGameEndStatus).AddTo(destroyDisposables);
            gameEndViewModel.OnRestartGame.Subscribe(HideGameObjectOnRestart).AddTo(destroyDisposables);
        }

        private void OnDestroy()
        {
            destroyDisposables?.Dispose();
            destroyDisposables = null;
        }

        private void SetGameEndStatus(string status)
        {
            gameEndStatusTMP.text = status;
            gameObject.SetActive(true);
        }

        private void HideGameObjectOnRestart(Unit _)
        {
            gameObject.SetActive(false);
        }
    }
}