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

        [ContextMenu("Spawn")]
        public virtual void Spawn()
        {
            if (prefab != null)
            {
                var spawned = Instantiate(prefab, target.position, target.rotation);
                spawned.transform.localScale = target.lossyScale;
                if (parent != null) spawned.transform.SetParent(parent.transform);
            }
            action?.Invoke();
        }

        public virtual void SpawnAt(Vector3 position)
        {
            if (prefab == null) return;

            var spawned = Instantiate(prefab, position, target.rotation);
            spawned.transform.localScale = target.lossyScale;
            if (parent != null) spawned.transform.SetParent(parent.transform);
        }

        public virtual void SpawnCustom(GameObject customPrefab)
        {
            if (customPrefab != null)
            {
                var spawned = Instantiate(customPrefab, target.position, target.rotation);
                spawned.transform.localScale = target.lossyScale;
                if (parent != null) spawned.transform.SetParent(parent.transform);
            }
            action?.Invoke();
        }
    }
}