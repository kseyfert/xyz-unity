using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Effects
{
    public class InfiniteBackgroundEffect : MonoBehaviour
    {
        [SerializeField] private Transform container;
        
        private Camera _camera;

        private Bounds _containerBounds;
        private Bounds _allBounds;

        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;
        private Vector3 _screenSize;

        private void Start()
        {
            var sprites = container.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;

            _boundsToTransformDelta = transform.position - _allBounds.center;
            _containerDelta = container.position - _allBounds.center;
            
            var cameraSingleton = SingletonMonoBehaviour.GetInstance<CameraSingleton>();
            _camera = cameraSingleton.GetComponentInChildren<Camera>();
        }

        private void LateUpdate()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero);
            var max = _camera.ViewportToWorldPoint(Vector3.one);
            _screenSize = new Vector3(max.x - min.x, max.y - min.y);

            _allBounds.center = transform.position - _boundsToTransformDelta;
            var camPosition = _camera.transform.position.x;
            var screenLeft = new Vector3(camPosition - _screenSize.x / 2, _containerBounds.center.y);
            var screenRight = new Vector3(camPosition + _screenSize.x / 2, _containerBounds.center.y);

            if (!_allBounds.Contains(screenLeft))
            {
                InstantiateContainer(_allBounds.min.x - _containerBounds.extents.x);
            }

            if (!_allBounds.Contains(screenRight))
            {
                InstantiateContainer(_allBounds.max.x + _containerBounds.extents.x);
            }
        }

        private void InstantiateContainer(float boundCenterX)
        {
            var newBounds = new Bounds(new Vector3(boundCenterX, _containerBounds.center.y), _containerBounds.size);
            _allBounds.Encapsulate(newBounds);

            _boundsToTransformDelta = transform.position - _allBounds.center;
            var newContainerXPos = boundCenterX + _containerDelta.x;
            var newPosition = new Vector3(newContainerXPos, container.transform.position.y);

            Instantiate(container, newPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            DrawBounds(_allBounds, Color.magenta);
        }
        
        private void DrawBounds(Bounds bounds, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(bounds.min, new Vector3(bounds.min.x, bounds.max.y));
            Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y), bounds.max);
            Gizmos.DrawLine(bounds.max, new Vector3(bounds.max.x, bounds.min.y));
            Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y), bounds.min);

            Gizmos.color = prevColor;
        }
    }
}