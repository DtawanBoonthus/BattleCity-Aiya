using BC.Shared.GameSessions;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace BC.UI
{
    public class GameModeView : MonoBehaviour
    {
        private IGameModeViewModel gameModeViewModel = null!;

        [SerializeField] private Button hostButton = null!;
        [SerializeField] private Button clientButton = null!;

        private CompositeDisposable? disposables;

        [Inject]
        private void Construct(IGameModeViewModel viewModel)
        {
            gameModeViewModel = viewModel;
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            disposables ??= new CompositeDisposable();
            gameModeViewModel.OnConnectSuccess.Subscribe(OnConnectSuccess).AddTo(disposables);
            hostButton.onClick.AddListener(OnClickHost);
            clientButton.onClick.AddListener(OnClickClient);
        }

        private void OnDisable()
        {
            disposables?.Dispose();
            disposables = null;
            hostButton.onClick.RemoveListener(OnClickHost);
            clientButton.onClick.RemoveListener(OnClickClient);
        }

        private void OnClickHost()
        {
            gameModeViewModel.ConnectGameAsync(GameMode.Host).Forget();
        }

        private void OnClickClient()
        {
            gameModeViewModel.ConnectGameAsync(GameMode.Client).Forget();
        }

        private void OnConnectSuccess(Unit _)
        {
            gameObject.SetActive(false);
        }
    }
}