using BC.Gameplay.Configs;
using BC.Shared.Inputs;
using BC.Shared.Spawners;
using Mirage;
using VContainer;

namespace BC.Gameplay.Tanks
{
    public class TankAttack : NetworkBehaviour
    {
        [Inject] private readonly IInputProvider inputProvider = null!;
        [Inject] private readonly ISpawnService<MirageNet> spawnService = null!;
        [Inject] private readonly ITankConfig tankConfig = null!;

        private void Update()
        {
            if (!HasAuthority)
            {
                return;
            }

            if (inputProvider.IsAttack)
            {
            }
        }
    }
}