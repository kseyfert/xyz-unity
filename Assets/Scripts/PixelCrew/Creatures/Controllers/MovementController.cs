using System;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class MovementController : AController
    {
        public delegate void MovementDelegate();

        public MovementDelegate onJumpStarted;
        public MovementDelegate onDoubleJumpStarted;
        public MovementDelegate onGrounded;
        public MovementDelegate onLongFallGrounded;
        
        [SerializeField] private Creature creature;
        
        [SerializeField] private LayerTriggerCheckComponent groundChecker;

        [SerializeField] private float speed = 7;
        [SerializeField] private float upperSpeedMultiplier = 1;
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

        private Vector3 _initialScale = Vector3.one;
        private int _lookingSide = 1;
        private int _normalSide = 1;

        private bool _isJumpRequested = false;
        private bool _isJumpCancelled = false;
        
        private bool _isJumpStarted = false;
        private bool _isDoubleJumpStarted = false;

        private bool _isKickbackRequested = false;
        private bool _isKickbackNow = false;

        private bool _speeding = false;

        private readonly Cooldown _cooldown = new Cooldown();

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

            _initialScale = _transform.localScale;
            
            LoadFromSession();
        }

        private void FixedUpdate()
        {
            if (!_cooldown.IsReady) return;
            
            _isKickbackNow = false;

            var isGrounded = IsGrounded();
            if (!_wasGrounded && isGrounded)
            {
                onGrounded?.Invoke();
                var velocity = _rb.velocity.magnitude;
                if (velocity >= longFallVelocity)
                {
                    _cooldown.SetMax(longFallFrozenTime);
                    
                    onLongFallGrounded?.Invoke();
                }
            }

            _wasGrounded = isGrounded;
            
            var velocityX = CalculateX();
            var velocityY = CalculateY();
            
            _rb.velocity = new Vector2(velocityX, velocityY);
            SaveToSession();

            if (_isKickbackRequested)
            {
                _cooldown.SetMax(kickbackFrozenTime);
                
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
            
            scaleVector.x = _initialScale.x * _lookingSide;
            scaleVector.y = _initialScale.y * _normalSide;
            
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

        public int GetDirection()
        {
            return Math.Sign(_direction.x);
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
                    onJumpStarted?.Invoke();
                } else if (canDoubleJump)
                {
                    velocityY = jumpSpeed;
                    _isDoubleJumpStarted = true;
                    onDoubleJumpStarted?.Invoke();
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

        public void AllowDoubleJump()
        {
            doubleJumpAllowed = true;
            SaveToSession();
        }

        public void SpeedUp()
        {
            if (_speeding) return;
            
            _speeding = true;
            speed *= upperSpeedMultiplier;
        }

        public void SpeedDown()
        {
            if (!_speeding) return;

            _speeding = false;
            speed /= upperSpeedMultiplier;
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

        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
            SetDirection(0);
            _rb.velocity = Vector2.zero;
            
            base.Die();
        }
    }
}