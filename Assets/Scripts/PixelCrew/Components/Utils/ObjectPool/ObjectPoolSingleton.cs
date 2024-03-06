using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Utils.ObjectPool
{
    public class ObjectPoolSingleton : SingletonMonoBehaviour
    {
        private readonly Dictionary<int, Queue<ObjectPoolItem>> _items = new Dictionary<int, Queue<ObjectPoolItem>>();

        public GameObject Get(GameObject go, Vector3 position, Quaternion rotation)
        {
            var id = go.GetInstanceID();
            var queue = RequireQueue(id);

            if (queue.Count > 0)
            {
                var elem = queue.Dequeue();

                elem.transform.position = position;
                elem.transform.rotation = rotation;
                
                elem.Enable();
                elem.Restart();

                return elem.gameObject;
            }

            var instance = Instantiate(go, position, rotation);
            var poolItem = instance.GetComponent<ObjectPoolItem>();
            if (poolItem == null) return instance;

            poolItem.transform.position = position;
            poolItem.transform.rotation = rotation;
            
            poolItem.Retain(id);
            poolItem.Enable();

            return instance;
        }

        private Queue<ObjectPoolItem> RequireQueue(int id)
        {
            if (!_items.TryGetValue(id, out var queue))
            {
                queue = new Queue<ObjectPoolItem>();
                _items.Add(id, queue);
            }

            return queue;
        }
        
        public void Release(int id, ObjectPoolItem poolItem)
        {
            var queue = RequireQueue(id);
            queue.Enqueue(poolItem);

            poolItem.Disable();
        }
    }
}