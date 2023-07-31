using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private GameObject prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            Instantiate(prefab, target.position, target.rotation);
        }

    }
}