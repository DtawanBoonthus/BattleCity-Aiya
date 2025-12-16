using BC.Gameplay.Configs;
using BC.Gameplay.Damageable;
using Cysharp.Threading.Tasks;
using Mirage;
using VContainer;
using VitalRouter;

namespace BC.Gameplay.Tanks
{
    public class TankHealth : NetworkBehaviour, IDamageable
    {
        [Inject] private readonly ICommandPublisher publisher = null!;
        [Inject] private readonly ITankConfig tankConfig = null!;

        [SyncVar] private int maxHealth;

        [SyncVar(hook = nameof(OnHpChanged))] private int currentHealth;

        private void Start()
        {
            if (!IsServer)
            {
                return;
            }

            maxHealth = tankConfig.MaxHealth;
            currentHealth = maxHealth;
        }

        [Server]
        public void TakeDamage(uint id, int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnHpChanged(int oldValue, int newValue)
        {
            UpdateHpAsync(NetId, newValue).Forget();
        }

        private async UniTask UpdateHpAsync(uint id, int hp)
        {
            await publisher.PublishAsync(new UpdateHpCommand(id, hp));
        }
    }
}