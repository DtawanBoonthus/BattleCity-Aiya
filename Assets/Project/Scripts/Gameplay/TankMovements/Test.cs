using BC.Shared.Networks;
using R3;
using UnityEngine;
using VContainer;

namespace BC.Gameplay.TankMovements
{
    public class Test : MonoBehaviour
    {
        [Inject] private readonly INetworkContext networkContext = null!;
        [Inject] private readonly INetworkLifecycle networkLifecycle = null!;
        [Inject] private readonly INetworkConfig networkConfig = null!;
        private CompositeDisposable? disposables;

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            disposables ??= new CompositeDisposable();

            networkConfig.Address
                .Subscribe(Debug.Log)
                .AddTo(disposables);
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            disposables?.Dispose();
        }

        [ContextMenu("Run T()")]
        public void T()
        {
            networkLifecycle.StartServer();
        }
    }
}