using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils.Checks
{
    public class AreaComponent : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float width;
        [SerializeField] private float height;
        
        [SerializeField] private AreaEvent onEnter;
        [SerializeField] private AreaEvent onExit;

        [SerializeField] private float outsideTimeout;
        [SerializeField] private AreaEvent onOutsideTimeout;

        [SerializeField] private float insideTimeout;
        [SerializeField] private AreaEvent onInsideTimeout;

        private bool _started;
        private Vector3 _center;
        private Bounds _bounds;

        private bool _isInside;
        private bool _wasInside;

        private float _lastTimeInside;
        private float _lastTimeOutside;

        private void Start()
        {
            _center = target.transform.position;
            CalcBounds();
            
            _started = true;
            
            if (insideTimeout <= 0 && outsideTimeout <= 0) return;
            
            if (IsInside()) _lastTimeInside = Time.time;
            else _lastTimeOutside = Time.time;
        }

        private void CalcBounds()
        {
            _bounds = new Bounds(_center, new Vector3(width, height));
            CheckIsInside();
        }

        public void ResetPosition()
        {
            _center = target.transform.position;
            CalcBounds();
        }

        public Vector3 GetCenter()
        {
            return _center;
        }

        private void Update()
        {
            CheckIsInside();
            if (_wasInside && !_isInside) TriggerExit();
            else if (!_wasInside && _isInside) TriggerEnter();
            _wasInside = _isInside;
            
            var time = Time.time;
            if (insideTimeout > 0 && _lastTimeInside > 0 && time - _lastTimeInside > insideTimeout)
            {
                onInsideTimeout?.Invoke(target);
                _lastTimeInside = 0;
            }

            if (outsideTimeout > 0 && _lastTimeOutside > 0 && time - _lastTimeOutside > outsideTimeout)
            {
                onOutsideTimeout?.Invoke(target);
                _lastTimeOutside = 0;
            }
        }

        private void CheckIsInside()
        {
            var position = target.transform.position;
            var point = _bounds.ClosestPoint(position);
            _isInside = point == position;
        }

        public bool IsInside()
        {
            return _isInside;
        }

        public bool IsOutside()
        {
            return !_isInside;
        }

        private void TriggerEnter()
        {
            onEnter?.Invoke(target);

            if (insideTimeout > 0) _lastTimeInside = Time.time;
            _lastTimeOutside = 0;
        }

        private void TriggerExit()
        {
            onExit?.Invoke(target);

            if (outsideTimeout > 0) _lastTimeOutside = Time.time;
            _lastTimeInside = 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_started)
            {
                _center = target.transform.position;
                CalcBounds();    
            }
            
            var x = _bounds.center.x - _bounds.size.x / 2;
            var y = _bounds.center.y - _bounds.size.y / 2;
            var w = _bounds.size.x;
            var h = _bounds.size.y;
            var rect = new Rect(x, y, w, h);
         
            var color = new Color(1, 0, 0, 0.2f);
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawSolidRectangleWithOutline(rect, color, color);
        }
#endif

        [Serializable]
        private class AreaEvent : UnityEvent<GameObject> {}
    }
}