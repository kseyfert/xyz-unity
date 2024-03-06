using PixelCrew.Components.Utils.ObjectPool;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected GameObject parent;
        [SerializeField] protected UnityEvent action;
        [SerializeField] protected bool usePool;

        private ObjectPoolSingleton _objectPoolSingleton;

        private void Awake()
        {
            _objectPoolSingleton = SingletonMonoBehaviour.GetOrCreateInstance<ObjectPoolSingleton>();
        }

        [ContextMenu("Spawn")]
        public virtual void Spawn()
        {
            if (prefab != null)
            {
                CreateObject(
                    prefab, 
                    target.position, 
                    target.rotation,
                    target.lossyScale,
                    parent
                );
            }
            action?.Invoke();
        }

        public virtual void SpawnAt(Vector3 position)
        {
            if (prefab == null) return;

            CreateObject(
                prefab, 
                position, 
                target.rotation,
                target.lossyScale,
                parent
            );
        }

        public virtual void SpawnCustom(GameObject customPrefab)
        {
            if (customPrefab != null)
            {
                CreateObject(
                    customPrefab, 
                    target.position, 
                    target.rotation,
                    target.lossyScale,
                    parent
                );
            }
            action?.Invoke();
        }

        protected GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale, GameObject parent)
        {
            GameObject go;
            
            if (usePool) go = _objectPoolSingleton.Get(prefab, position, rotation);
            else go = Instantiate(prefab, position, rotation);

            go.transform.localScale = localScale;
            if (parent != null) go.transform.SetParent(parent.transform);
            else go.transform.SetParent(_objectPoolSingleton.gameObject.transform);

            return go;
        }
    }
}