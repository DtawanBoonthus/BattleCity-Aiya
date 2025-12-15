using BC.Gameplay.Configs;
using Mirage;
using UnityEngine;
using VContainer;

namespace BC.Gameplay.Tanks
{
    public class TankRender : NetworkBehaviour
    {
        [Inject] private readonly IGameplayConfig gameplayConfig = null!;
        [SerializeField] private SpriteRenderer spriteRenderer = null!;

        [ClientRpc]
        public void RPC_SetSprite(int tankSpriteId)
        {
            if (!IsClient)
            {
                return;
            }

            spriteRenderer.sprite = gameplayConfig.TanksSprites[tankSpriteId];
        }
    }
}