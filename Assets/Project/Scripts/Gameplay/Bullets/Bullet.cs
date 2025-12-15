using BC.Gameplay.Configs;
using BC.Shared.Spawners;
using Mirage;
using UnityEngine;
using VContainer;

namespace BC.Gameplay
{
    public class Bullet : NetworkBehaviour
    {
        [Inject] private readonly IBulletConfig bulletConfig = null!;
        [Inject] private readonly ISpawnService<MirageNet> spawnService = null!;

        private Vector2 direction;

        [Server]
        public void Init(Vector2 direction)
        {
            this.direction = direction.normalized;
        }


        private void Update()
        {
            if (!IsServer)
            {
                return;
            }

            var distance = bulletConfig.Speed * Time.deltaTime;
            Vector2 start = transform.position;
            Vector2 end = start + direction * distance;

            var hit = Physics2D.Raycast(start, direction, distance, bulletConfig.HitMask);

            if (ProcessHit(hit))
            {
                return;
            }

            transform.position = end;
        }

        private bool ProcessHit(RaycastHit2D hit)
        {
            if (hit.collider == null)
            {
                return false;
            }

            // TODO: damage / wall logic
            Destroy();
            return true;
        }

        [Server]
        private void Destroy()
        {
            if (!IsServer)
            {
                return;
            }

            spawnService.Despawn(gameObject);
        }
    }
}