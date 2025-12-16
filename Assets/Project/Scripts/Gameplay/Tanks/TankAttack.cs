using System.Collections.Generic;
using BC.Gameplay.Configs;
using BC.Gameplay.Damageable;
using BC.Shared.Inputs;
using BC.Shared.Spawners;
using Mirage;
using UnityEngine;
using VContainer;

namespace BC.Gameplay.Tanks
{
    public class TankAttack : NetworkBehaviour
    {
        [SerializeField] private Transform firePosition = null!;

        [Inject] private readonly IInputProvider inputProvider = null!;
        [Inject] private readonly ISpawnService<Normal> spawnService = null!;
        [Inject] private readonly ITankConfig tankConfig = null!;
        [Inject] private readonly IGameplayConfig gameplayConfig = null!;
        [Inject] private readonly BulletVisualService bulletVisualService = null!;
        [Inject] private readonly IBulletConfig bulletConfig = null!;

        private double lastFireTime;
        private uint bulletCounter;

        private struct ServerBullet
        {
            public uint BulletId;
            public Vector2 Position;
            public Vector2 Direction;
            public uint OwnerId;
        }

        private readonly List<ServerBullet> activeBullets = new();


        private void Update()
        {
            if (!HasAuthority)
            {
                return;
            }

            if (inputProvider.IsAttack)
            {
                RPC_Fire();
            }
        }

        [ServerRpc]
        private void RPC_Fire()
        {
            if (!CanFire())
            {
                return;
            }

            lastFireTime = NetworkTime.Time;

            var bulletId = ++bulletCounter;
            Vector2 startPosition = firePosition.position;
            var startRotation = firePosition.rotation;
            Vector2 direction = firePosition.right;


            activeBullets.Add(new ServerBullet
            {
                BulletId = bulletId,
                Position = startPosition,
                Direction = direction.normalized,
                OwnerId = NetId
            });

            Rpc_SpawnBulletVisual(bulletId, startPosition, startRotation, direction);
        }

        private void FixedUpdate()
        {
            if (!IsServer)
            {
                return;
            }

            var distance = bulletConfig.Speed * Time.fixedDeltaTime;

            for (var i = activeBullets.Count - 1; i >= 0; i--)
            {
                var bullet = activeBullets[i];
                Vector2 start = bullet.Position;
                Vector2 direction = bullet.Direction;

                var hit = Physics2D.Raycast(start, direction, distance, bulletConfig.HitMask);

                if (hit.collider != null)
                {
                    if (ProcessBulletHit(bullet, hit))
                    {
                        activeBullets.RemoveAt(i);
                        continue;
                    }
                }

                bullet.Position += direction * distance;
                activeBullets[i] = bullet;
            }
        }

        [Server]
        private bool ProcessBulletHit(ServerBullet bullet, RaycastHit2D hit)
        {
            var damageable = hit.collider.GetComponentInParent<IDamageable>();

            if (damageable == null)
            {
                Rpc_DespawnBulletVisual(bullet.BulletId);
                return true;
            }

            var networkBehaviour = hit.collider.GetComponentInParent<NetworkBehaviour>();
            if (networkBehaviour == null || networkBehaviour.NetId == bullet.OwnerId)
            {
                return false;
            }

            damageable.TakeDamage(networkBehaviour.NetId, bulletConfig.Damage);
            Rpc_DespawnBulletVisual(bullet.BulletId);
            return true;
        }

        [ClientRpc]
        private void Rpc_SpawnBulletVisual(uint bulletId, Vector2 position, Quaternion startRotation, Vector2 direction)
        {
            var bulletObj = spawnService.Spawn(gameplayConfig.BulletPrefab, position, startRotation);

            var visual = bulletObj.GetComponent<BulletVisual>();
            visual.Init(position, direction, bulletConfig.Speed);
            bulletVisualService.AddBulletVisual(bulletId, visual);
        }

        [ClientRpc]
        private void Rpc_DespawnBulletVisual(uint bulletId)
        {
            bulletVisualService.DespawnBulletVisual(bulletId);
        }

        [Server]
        private bool CanFire()
        {
            return NetworkTime.Time - lastFireTime >= tankConfig.FireCooldown;
        }
    }
}