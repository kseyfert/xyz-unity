using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] private float speed = 1;

        [Header("Jump")] 
        [SerializeField] private float jumpForce = 1;
        [SerializeField] private float jumpCoolDown = 0.5f;

        [Header("Debug")] [SerializeField] private bool debugEnabled = true;
        [SerializeField] private float debugSphereRadius;
        [SerializeField] private Vector3 debugSpherePosition;

        private Vector3 _direction;
        private Rigidbody2D _rb;
        private LayerChecker _groundChecker;

        public void SetDirection(float directionX)
        {
            SetDirection(new Vector2(directionX, 0));
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = new Vector3(Math.Sign(direction.x), Math.Sign(direction.y), 0);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _groundChecker = GetComponentInChildren<LayerChecker>();
        }

        private void FixedUpdate()
        {
            var velocity = _direction * speed;
            _rb.velocity = new Vector2(velocity.x, _rb.velocity.y);

            var isJumpPressed = _direction.y > 0;
            if (isJumpPressed && IsGrounded())
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (!isJumpPressed && _rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * jumpCoolDown);
            }
        }

        private bool IsGrounded()
        {
            if (_groundChecker == null) return true;
            return _groundChecker.IsTouchingLayer() && _rb.velocity.y <= 0;
        }

        private void OnDrawGizmos()
        {
            if (debugEnabled)
            {
                Gizmos.color = IsGrounded() ? Color.green : Color.red;
                Gizmos.DrawSphere(transform.position + debugSpherePosition, debugSphereRadius);
            }
        }
    }
}