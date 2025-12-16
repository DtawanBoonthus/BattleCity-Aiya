using BC.Gameplay.Configs;
using BC.Shared.Inputs;
using Mirage;
using R3;
using UnityEngine;
using VContainer;

namespace BC.Gameplay.Tanks
{
    public class TankMovement : NetworkBehaviour
    {
        [SerializeField] private Rigidbody2D rb2D = null!;
        [SerializeField] private TankHitReactionController hitReactionController = null!;

        [Inject] private readonly IInputProvider inputProvider = null!;
        [Inject] private readonly ITankConfig tankConfig = null!;

        private Vector2 currentInput;
        private CompositeDisposable? destroyDisposables;

        private void Start()
        {
            destroyDisposables ??= new CompositeDisposable();
        }

        private void OnDestroy()
        {
            destroyDisposables?.Dispose();
            destroyDisposables = null;
        }

        private void Update()
        {
            if (!HasAuthority || hitReactionController.IsStaggered)
            {
                return;
            }

            RPC_Input(inputProvider.Move);
        }

        private void FixedUpdate()
        {
            if (!IsServer || hitReactionController.IsStaggered)
            {
                return;
            }

            ServerMove();
            ServerUpdateRotation();
        }

        [ServerRpc]
        private void RPC_Input(Vector2 moveInput)
        {
            currentInput = Vector2.ClampMagnitude(moveInput, 1f);
        }

        [Server]
        private void ServerMove()
        {
            var position = rb2D.position + currentInput * tankConfig.StartMoveSpeed * Time.fixedDeltaTime;
            rb2D.MovePosition(position);
        }

        [Server]
        private void ServerUpdateRotation()
        {
            if (currentInput == Vector2.zero)
            {
                return;
            }

            var angle = Mathf.Atan2(currentInput.y, currentInput.x) * Mathf.Rad2Deg;

            angle = Mathf.Round(angle / 45f) * 45f;

            rb2D.SetRotation(angle);
        }
    }
}