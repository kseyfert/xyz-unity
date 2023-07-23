using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Hero
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Hero hero;
        [SerializeField] private LayerChecker groundChecker;

        [SerializeField] private float speed = 7;
        [SerializeField] private float jumpSpeed = 14;
        [SerializeField] private float jumpCooldown = 0.5f;
        [SerializeField] private bool debugEnabled = true;

        private Rigidbody2D _rb;
        private Transform _transform;

        private bool _isJumpRequested = false;
        private bool _isJumpCancelled = false;
        
        private bool _isJumpStarted = false;
        private bool _isDoubleJumpStarted = false;

        private Vector2 _direction;

        private void Awake()
        {
            _rb = hero.GetComponent<Rigidbody2D>();
            _transform = hero.GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            var velocityX = CalculateX();
            var velocityY = CalculateY();

            _rb.velocity = new Vector2(velocityX, velocityY);
            
            if (velocityX > 0) _transform.rotation = Quaternion.Euler(0, 0, 0);
            if (velocityX < 0) _transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        public void SetDirection(float directionX)
        {
            _direction = new Vector3(Math.Sign(directionX), 0, 0);
        }

        public void SetDirection(Vector2 direction)
        {
            SetDirection(direction.x);
        }
        
        public void SetJump(bool value)
        {
            if (value) DoJump();
            else CancelJump();
        }

        private void DoJump()
        {
            _isJumpRequested = true;
        }

        private void CancelJump()
        {
            _isJumpCancelled = true;
        }

        private float CalculateY()
        {
            var velocityY = _rb.velocity.y;

            var isGrounded = IsGrounded();
            var isFallingDown = velocityY < 0 && !isGrounded;
            var isGoingUp = velocityY > 0 && !isGrounded;

            if (isGrounded)
            {
                _isJumpStarted = false;
                _isDoubleJumpStarted = false;
            }
            
            var canJump = isGrounded;
            var canDoubleJump = !isGrounded && !_isDoubleJumpStarted;

            if (_isJumpRequested)
            {
                _isJumpRequested = false;

                if (canJump)
                {
                    velocityY = jumpSpeed;
                    _isJumpStarted = true;
                }

                if (canDoubleJump)
                {
                    velocityY = jumpSpeed;
                    _isDoubleJumpStarted = true;
                }
            }

            if (_isJumpCancelled)
            {
                _isJumpCancelled = false;

                if (isGoingUp && (_isJumpStarted || _isDoubleJumpStarted))
                {
                    velocityY *= jumpCooldown;
                }
            }

            return velocityY;
        }

        private float CalculateX()
        {
            return _direction.x * speed;
        }

        public bool IsGrounded()
        {
            return groundChecker.IsTouchingLayer();
        }
        
        private void OnDrawGizmos()
        {
            if (debugEnabled)
            {
                Gizmos.color = IsGrounded() ? Color.green : Color.red;
                Gizmos.DrawSphere(transform.position, 0.5f);
            }
        }
    }
}