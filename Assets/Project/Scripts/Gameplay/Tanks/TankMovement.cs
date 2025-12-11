using BC.Shared.Inputs;
using Mirage;
using UnityEngine;

namespace BC.Gameplay.Tanks
{
    public class TankMovement : NetworkBehaviour
    {
        private IInputProvider inputProvider = null!;

        public void Init(IInputProvider inputProvider)
        {
            if (!IsLocalPlayer)
            {
                return;
            }

            this.inputProvider = inputProvider;
        }

        private void Update()
        {
            if (!IsLocalPlayer)
            {
                return;
            }

            RPC_Input(inputProvider.Move);
        }

        [ServerRpc]
        private void RPC_Input(Vector2 moveInput)
        {
            ServerMove(moveInput);
        }

        private void ServerMove(Vector2 moveInput)
        {
        }
    }
}