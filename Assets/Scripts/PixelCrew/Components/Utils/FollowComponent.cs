using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class FollowComponent : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool isAbsolute;
        [SerializeField] private bool followScale;

        private Transform _transform;
        private Vector3 _delta;
        
        private float _scaleDeltaX;
        private float _scaleDeltaY;
        private float _scaleDeltaZ;

        private void Start()
        {
            _transform = transform;
            
            _delta = _transform.position - target.position;

            var localScale = _transform.localScale;
            var scale = target.localScale;
            _scaleDeltaX = localScale.x / scale.x;
            _scaleDeltaY = localScale.y / scale.y;
            _scaleDeltaZ = localScale.z / scale.z;
        }
        
        private void Update()
        {
            if (target == null)
            {
                enabled = false;
                return;
            }
            
            if (isAbsolute) _transform.position = target.position;
            else _transform.position = target.position + _delta;

            if (followScale)
            {
                Vector3 newScale = Vector3.zero;
                var localScale = target.localScale;
                newScale.x = _scaleDeltaX * localScale.x;
                newScale.y = _scaleDeltaY * localScale.y;
                newScale.z = _scaleDeltaZ * localScale.z;

                _transform.localScale = newScale;
            }
        }
    }
}