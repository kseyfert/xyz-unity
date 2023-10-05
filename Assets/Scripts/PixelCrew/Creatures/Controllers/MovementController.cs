using System;
using PixelCrew.Components.Utils.Checks;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class MovementController : MonoBehaviour
    {
        public event EventHandler OnJumpStarted;
        public event EventHandler OnGrounded;
        public event EventHandler OnLongFallGrounded;
        
        [SerializeField] private Creature creature;
        
        [SerializeField] private LayerTriggerCheckComponent groundChecker;

        [SerializeField] private float speed = 7;
        [SerializeField] private float jumpSpeed = 14;
        [SerializeField] private float jumpCooldown = 0.5f;
        [SerializeField] private bool debugEnabled = true;
        [SerializeField] private bool doubleJumpAllowed = false;
        [SerializeField] private bool infiniteJumpAllowed = false;
        [SerializeField] private float kickbackPower = 3;
        [SerializeField] private float kickbackFrozenTime = 1;
        [SerializeField] private float longFallVelocity = 15;
        [SerializeField] private float longFallFrozenTime = 0.3f;

        private SessionController _sessionController;

        private Rigidbody2D _rb;
        private Transform _transform;

        private int _lookingSide = 1;
        private int _normalSide = 1;

        private bool _isJumpRequested = false;
        private bool _isJumpCancelled = false;
        
        private bool _isJumpStarted = false;
        private bool _isDoubleJumpStarted = false;

        private bool _isKickbackRequested = false;
        private bool _isKickbackNow = false;

        private float _frozenTimer = 0;

        private Vector2 _direction;

        private bool _wasGrounded = true;

        private void LoadFromSession()
        {
            if (_sessionController == null) return;

            doubleJumpAllowed = _sessionController.GetModel().isDoubleJumpAllowed;
            infiniteJumpAllowed = _sessionController.GetModel().isInfiniteJumpAllowed;
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;

            _sessionController.GetModel().isDoubleJumpAllowed = doubleJumpAllowed;
            _sessionController.GetModel().isInfiniteJumpAllowed = infiniteJumpAllowed;
        }
        
        private void Start()
        {
            _rb = creature.Rigidbody2D;
            _transform = creature.Transform;
            _sessionController = creature.SessionController;
            
            LoadFromSession();
        }

        private void FixedUpdate()
        {
            if (Time.time < _frozenTimer) return;
            _isKickbackNow = false;

            var isGrounded = IsGrounded();
            if (!_wasGrounded && isGrounded)
            {
                OnGrounded?.Invoke(this, EventArgs.Empty);
                var velocity = _rb.velocity.magnitude;
                if (velocity >= longFallVelocity)
                {
                    _frozenTimer = Time.time + longFallFrozenTime;
                    OnLongFallGrounded?.Invoke(this, EventArgs.Empty);
                }
            }

            _wasGrounded = isGrounded;
            
            var velocityX = CalculateX();
            var velocityY = CalculateY();
            
            _rb.velocity = new Vector2(velocityX, velocityY);
            SaveToSession();

            if (_isKickbackRequested)
            {
                _frozenTimer = Time.time + kickbackFrozenTime;
                _isKickbackRequested = false;
                _isKickbackNow = true;
                return;
            }

            if (velocityX > 0)
            {
                _lookingSide = 1;
            }

            if (velocityX < 0)
            {
                _lookingSide = -1;
            }

            var scaleVector = _transform.localScale;
            
            scaleVector.x = _lookingSide;
            scaleVector.y = _normalSide;
            
            _transform.localScale = scaleVector;
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
            var canDoubleJump = infiniteJumpAllowed || doubleJumpAllowed && !isGrounded && !_isDoubleJumpStarted;

            if (_isJumpRequested)
            {
                _isJumpRequested = false;

                if (canJump)
                {
                    velocityY = jumpSpeed;
                    _isJumpStarted = true;
                    OnJumpStarted?.Invoke(this, EventArgs.Empty);
                } else if (canDoubleJump)
                {
                    velocityY = jumpSpeed;
                    _isDoubleJumpStarted = true;
                    OnJumpStarted?.Invoke(this, EventArgs.Empty);
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
            SaveToSession();
        }

        public void Kickback()
        {
            if (!_isKickbackNow) _isKickbackRequested = true;
        }

        public void Freeze(float time = 1)
        {
            _frozenTimer = Time.time + time;
        }

        public void Unfreeze()
        {
            _frozenTimer = Time.time - 100;
        }

        public void AllowDoubleJump()
        {
            doubleJumpAllowed = true;
            SaveToSession();
        }

        public void Inverse()
        {
            _normalSide = -_normalSide;
            _rb.gravityScale = -_rb.gravityScale;
            jumpSpeed = -jumpSpeed;
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