using BC.Gameplay.Configs;
using BC.Gameplay.Damageable;
using Cysharp.Threading.Tasks;
using Mirage;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace BC.Gameplay.Tanks
{
    public class TankHealth : NetworkBehaviour, IDamageable
    {
        [Inject] private readonly ICommandPublisher publisher = null!;
        [Inject] private readonly ITankConfig tankConfig = null!;

        [SyncVar(hook = nameof(OnHpChanged))] private int currentHealth;

        private void Start()
        {
            if (!IsServer)
            {
                return;
            }

            currentHealth = tankConfig.MaxHealth;
        }

        [Server]
        public void TakeDamage(uint id, int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                var playerLoseId = Identity.Owner.Identity.NetId;
                var playerWinner = Server.AllPlayers.FirstOrDefault(networkPlayer => networkPlayer.Identity.NetId != playerLoseId);

                if (playerWinner == null)
                {
                    Debug.LogError("PlayerWinner is null.");
                    return;
                }

                DeadAsync(playerWinner.Identity.NetId, playerLoseId).Forget();

                gameObject.SetActive(false);
            }
        }

        private void OnHpChanged(int oldValue, int newValue)
        {
            if (!IsClient)
            {
                return;
            }

            UpdateHpAsync(NetId, newValue).Forget();
        }

        private async UniTask UpdateHpAsync(uint id, int hp)
        {
            await publisher.PublishAsync(new UpdateHpCommand(id, hp));
        }

        private async UniTask DeadAsync(uint playerWinnerId, uint playerLoseId)
        {
            await publisher.PublishAsync(new TankDeadCommand(playerWinnerId, playerLoseId));
        }
    }
}