using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject parent;
        [SerializeField] private UnityEvent action;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            if (prefab != null)
            {
                var spawned = Instantiate(prefab, target.position, target.rotation);
                spawned.transform.localScale = target.lossyScale;
                if (parent != null) spawned.transform.SetParent(parent.transform);
            }
            action?.Invoke();
        }

        public void SpawnAt(Vector3 position)
        {
            if (prefab == null) return;

            var spawned = Instantiate(prefab, position, target.rotation);
            spawned.transform.localScale = target.lossyScale;
            if (parent != null) spawned.transform.SetParent(parent.transform);
        }

    }
}