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
        [SerializeField] private bool infiniteJumpAllowed = false;
        [SerializeField] private float kickbackPower = 3;
        [SerializeField] private float kickbackFrozenTime = 1;

        private Rigidbody2D _rb;
        private Transform _transform;

        private int _lookingSide = 1;

        private bool _isJumpRequested = false;
        private bool _isJumpCancelled = false;
        
        private bool _isJumpStarted = false;
        private bool _isDoubleJumpStarted = false;

        private bool _isKickbackRequested = false;

        private float _frozenTimer = 0;

        private Vector2 _direction;

        private void Awake()
        {
            _rb = hero.GetComponent<Rigidbody2D>();
            _transform = hero.GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            if (Time.time < _frozenTimer) return;
            
            var velocityX = CalculateX();
            var velocityY = CalculateY();
            
            _rb.velocity = new Vector2(velocityX, velocityY);

            if (_isKickbackRequested)
            {
                _frozenTimer = Time.time + kickbackFrozenTime;
                _isKickbackRequested = false;
                return;
            }

            if (velocityX > 0)
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
                _lookingSide = 1;
            }

            if (velocityX < 0)
            {
                _transform.rotation = Quaternion.Euler(0, 180, 0);
                _lookingSide = -1;
            }
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
            if (_isKickbackRequested)
            {
                return kickbackPower;
            }
            var velocityY = _rb.velocity.y;

            var isGrounded = IsGrounded();
            var isGoingUp = velocityY > 0 && !isGrounded;

            if (isGrounded)
            {
                _isJumpStarted = false;
                _isDoubleJumpStarted = false;
            }
            
            var canJump = isGrounded;
            var canDoubleJump = infiniteJumpAllowed || !isGrounded && !_isDoubleJumpStarted;

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
            if (_isKickbackRequested)
            {
                return -1 * _lookingSide * kickbackPower;
            }
            return _direction.x * speed;
        }

        public bool IsGrounded()
        {
            return groundChecker.IsTouchingLayer();
        }

        public bool IsJumping()
        {
            return _isJumpStarted;
        }

        public bool IsDoubleJumping()
        {
            return _isDoubleJumpStarted;
        }

        public void AllowInfiniteJump()
        {
            infiniteJumpAllowed = true;
        }

        public void Kickback()
        {
            _isKickbackRequested = true;
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