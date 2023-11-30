using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class ReplaceComponent : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public void Replace()
        {
            if (prefab == null) return;

            var t = transform;
            var spawned = Instantiate(prefab, t.position, t.rotation);
            spawned.transform.localScale = t.lossyScale;
            spawned.transform.SetParent(t.parent);
            
            Destroy(gameObject);
        }
    }
}