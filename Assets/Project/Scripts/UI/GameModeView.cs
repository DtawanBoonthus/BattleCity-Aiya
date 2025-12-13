using BC.Gameplay;
using BC.Shared.GameSessions;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VitalRouter;

namespace BC.UI
{
    public class GameModeView : MonoBehaviour
    {
        [Inject] private readonly ICommandPublisher publisher = null!;

        [SerializeField] private Button hostButton = null!;
        [SerializeField] private Button clientButton = null!;

        private void OnEnable()
        {
            hostButton.onClick.AddListener(OnClickHost);
            clientButton.onClick.AddListener(OnClickClient);
        }

        private void OnDisable()
        {
            hostButton.onClick.RemoveListener(OnClickHost);
            clientButton.onClick.RemoveListener(OnClickClient);
        }

        private void OnClickHost()
        {
            publisher.PublishAsync(new GameModeCommand(GameMode.Host));
        }

        private void OnClickClient()
        {
            publisher.PublishAsync(new GameModeCommand(GameMode.Client));
        }
    }
}