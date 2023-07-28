using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform destination;

        public void Teleport(GameObject target)
        {
            target.transform.position = destination.position;
        }

    }
}