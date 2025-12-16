using Mirage;
using R3;
using TMPro;
using UnityEngine;
using VContainer;

namespace BC.UI
{
    public class TankWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpTMP = null!;

        [Inject] private readonly TankWorldUIRouter router = null!;
        private CompositeDisposable? destroyDisposables;

        private uint id;

        private void Start()
        {
            id = GetComponentInParent<NetworkBehaviour>().NetId;
            destroyDisposables ??= new CompositeDisposable();
            router.Hp.Subscribe(UpdateHp).AddTo(destroyDisposables);
        }

        private void OnDestroy()
        {
            destroyDisposables?.Dispose();
            destroyDisposables = null;
        }

        private void UpdateHp((uint id, int hp) updateTuple)
        {
            if (id != updateTuple.id)
            {
                return;
            }

            hpTMP.text = $"HP: {updateTuple.hp}";
        }
    }
}