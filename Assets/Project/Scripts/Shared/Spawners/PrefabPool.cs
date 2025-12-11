using System.Collections.Generic;
using UnityEngine;

namespace BC.Shared.Spawners
{
    public class PrefabPool
    {
        private readonly Queue<GameObject> available = new();
        private readonly List<GameObject> active = new();

        private readonly GameObject prefab;
        private readonly int maxSize;
        private readonly bool unlimited;
        private readonly Transform root;

        public PrefabPool(GameObject prefab, int startSize, int maxSize, bool unlimited, Transform root)
        {
            this.prefab = prefab;
            this.maxSize = maxSize;
            this.unlimited = unlimited;
            this.root = root;

            for (int i = 0; i < startSize; i++)
            {
                var obj = Object.Instantiate(prefab, root);
                obj.SetActive(false);
                available.Enqueue(obj);
            }
        }

        public GameObject Spawn(Vector3 position, Quaternion quaternion)
        {
            GameObject gameObject;

            if (available.Count > 0)
            {
                gameObject = available.Dequeue();
            }
            else
            {
                int total = active.Count + available.Count;

                if (unlimited || total < maxSize)
                {
                    gameObject = Object.Instantiate(prefab, root);
                }
                else
                {
                    gameObject = active[0];
                    active.RemoveAt(0);
                }
            }

            gameObject.transform.SetPositionAndRotation(position, quaternion);
            gameObject.SetActive(true);
            active.Add(gameObject);

            return gameObject;
        }

        public void Despawn(GameObject gameObject)
        {
            gameObject.SetActive(false);
            active.Remove(gameObject);
            available.Enqueue(gameObject);
        }
    }
}