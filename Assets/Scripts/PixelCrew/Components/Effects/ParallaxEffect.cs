using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Effects
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float effectValue;
        
        private Camera _camera;
        private float _startX;
        private bool _init = false;
        
        private void Start()
        {
            _startX = transform.position.x;
            
            var cameraSingleton = SingletonMonoBehaviour.GetInstance<CameraSingleton>();
            _camera = cameraSingleton.GetComponentInChildren<Camera>();

            _init = _camera != null;
        }

        private void LateUpdate()   
        {
            if (!_init) return;
            
            var t = transform;
            
            var currentPosition = t.position;
            var deltaX = _camera.transform.position.x * effectValue;

            t.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}