using BC.Shared.Spawners;
using UnityEngine;
using VContainer;

namespace BC.Gameplay
{
    public class BulletVisual : MonoBehaviour
    {
        [Inject] private readonly ISpawnService<Normal> spawnService = null!;

        private Vector2 direction;
        private float speed;

        public void Init(Vector3 firePosition, Vector2 direction, float speed)
        {
            this.direction = direction.normalized;
            transform.position = firePosition;
            this.speed = speed;
        }

        private void Update()
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        public void Destroy()
        {
            spawnService.Despawn(gameObject);
        }
    }
}