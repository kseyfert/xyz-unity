using System;
using UnityEngine;

namespace PixelCrew.Components.Movings
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [Serializable]
    public abstract class AMovingComponent : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private Transform _transform;
        private Rigidbody2D _rigidbody2D;

        private int _direction = 0;
        private Vector2 _original;
        private float _time = 0;
        
        protected virtual void Start()
        {
            _transform = GetComponent<Transform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _original = _transform.position;
        }

        protected virtual void FixedUpdate()
        {
            if (_direction == 0) _direction = Math.Sign(_transform.lossyScale.x);

            _rigidbody2D.MovePosition(CalcNewPosition());
            
            _time += Time.fixedDeltaTime;
        }

        protected Vector2 GetOriginal()
        {
            return _original;
        }

        protected Vector2 GetCurrent()
        {
            return _transform.position;
        }

        protected int GetDirection()
        {
            return _direction;
        }

        protected float GetSpeed()
        {
            return speed;
        }

        protected float GetTime()
        {
            return _time;
        }

        protected abstract Vector2 CalcNewPosition();
    }
}