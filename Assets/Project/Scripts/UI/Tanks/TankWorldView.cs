using System.Collections.Generic;
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
        [SerializeField] private TextMeshProUGUI staggerTMP = null!;
        [SerializeField] private TextMeshProUGUI iframeTMP = null!;

        [Inject] private readonly TankWorldUIRouter router = null!;
        private CompositeDisposable? destroyDisposables;

        private uint id;

        private void Start()
        {
            id = GetComponentInParent<NetworkBehaviour>().NetId;
            destroyDisposables ??= new CompositeDisposable();
            router.Hps.Subscribe(UpdateHp).AddTo(destroyDisposables);
            router.Staggers.Subscribe(UpdateStagger).AddTo(destroyDisposables);
            router.Iframes.Subscribe(UpdateIframe).AddTo(destroyDisposables);
        }

        private void OnDestroy()
        {
            destroyDisposables?.Dispose();
            destroyDisposables = null;
        }

        private void UpdateHp(IReadOnlyDictionary<uint, int> playerHp)
        {
            hpTMP.text = $"HP: {playerHp[id]}";
        }

        private void UpdateStagger(IReadOnlyDictionary<uint, int> playerStagger)
        {
            if (playerStagger[id] <= 0)
            {
                staggerTMP.text = "Stagger: 0";
                return;
            }

            staggerTMP.text = $"Stagger: {playerStagger[id]}";
        }

        private void UpdateIframe(IReadOnlyDictionary<uint, float> playerIframe)
        {
            if (playerIframe[id] <= 0)
            {
                iframeTMP.text = "Iframe: 0";
                return;
            }

            iframeTMP.text = $"Iframe: {playerIframe[id]}";
        }
    }
}