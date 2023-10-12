using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Transform _transform;
        private Rigidbody2D _rigidbody2D;

        private int _direction = 0;
        
        private void Start()
        {
            _transform = GetComponent<Transform>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_direction == 0) _direction = Math.Sign(_transform.lossyScale.x);
            
            var position = _transform.position;
            position.x += _direction * speed;
            _rigidbody2D.MovePosition(position);
        }
    }
}