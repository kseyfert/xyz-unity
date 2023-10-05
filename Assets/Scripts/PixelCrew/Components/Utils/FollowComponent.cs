using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class FollowComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool isAbsolute;

        private Vector3 _delta;

        private void Start()
        {
            _delta = transform.position - target.position;
        }
        
        private void Update()
        {
            if (target == null)
            {
                enabled = false;
                return;
            }
            
            if (isAbsolute) transform.position = target.position;
            else transform.position = target.position + _delta;
        }
    }
}