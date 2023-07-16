using UnityEngine;

namespace PixelCrew.Components
{
    public class LayerChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        private Collider2D _collider;
        private bool _isTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layerMask);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layerMask);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layerMask);
        }

        public bool IsTouchingLayer()
        {
            return _isTouchingLayer;
        }
    }
}