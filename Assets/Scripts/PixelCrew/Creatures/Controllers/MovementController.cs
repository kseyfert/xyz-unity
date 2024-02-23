using System;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
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
        
        [SerializeField] private LayerTriggerCheckComponent groundChecker;

        [SerializeField] private float speed = 7;
        [SerializeField] private float upperSpeedMultiplier = 1;
        [SerializeField] private float jumpSpeed = 14;
        [SerializeField] private float jumpCooldown = 0.5f;
        [SerializeField] private bool debugEnabled = true;
        [SerializeField] private float kickbackPower = 3;
        [SerializeField] private float kickbackFrozenTime = 1;
        [SerializeField] private float longFallVelocity = 15;
        [SerializeField] private float longFallFrozenTime = 0.3f;

        private InventoryData _inventory;
        private QuickInventoryModel _quickInventory;
        private PerksModel _perksModel;
        private StatsModel _statsModel;

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

        private float _speedingC = 1f;

        private readonly Cooldown _cooldown = new Cooldown();

        private Vector2 _direction;

        private bool _wasGrounded = true;
        
        private void Start()
        {
            _rb = Creature.Rigidbody2D;
            _transform = Creature.Transform;
            
            var sessionController = Creature.SessionController;
            if (sessionController != null) _inventory = sessionController.GetModel().inventory;
            if (sessionController != null) _quickInventory = sessionController.GetQuickInventory();
            if (sessionController != null) _perksModel = sessionController.GetPerksModel();
            if (sessionController != null) _statsModel = sessionController.GetStatsModel();

            _initialScale = _transform.localScale;

            _statsModel?.SubscribeAndInvoke(() => speed = _statsModel.GetValue(StatId.Speed));
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
            var canDoubleJump = IsInfiniteJumpAllowed() || IsDoubleJumpAllowed() && !isGrounded && !_isDoubleJumpStarted;

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
            return _direction.x * CalculateSpeed();
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
            _inventory?.Add(PlayerData.InfiniteJumper, 1);
        }

        public void Kickback()
        {
            if (!_isKickbackNow) _isKickbackRequested = true;
        }

        public void AllowDoubleJump()
        {
            _inventory?.Add(PlayerData.DoubleJumper, 1);
        }

        public void SpeedUp()
        {
            if (_speedingC > 1) return;
            
            _speedingC = upperSpeedMultiplier;
        }

        public void SpeedUp(float value)
        {
            if (_speedingC > 1) return;

            _speedingC = value;
        }

        public void SpeedDown()
        {
            if (_speedingC < 1.01f) return;

            _speedingC = 1f;
        }

        public void ApplyPotion()
        {
            if (_inventory == null) return;

            var selectedItem = _quickInventory.SelectedItem;

            if (!DefsFacade.I.Potions.Has(selectedItem.id)) return;
            
            var potionDef = DefsFacade.I.Potions.Get(selectedItem.id);
            if (potionDef.Type != PotionType.Speed) return;
            
            _inventory.Apply(selectedItem.id, -1);

            var power = potionDef.Power;
            SpeedUp(power);
            this.SetTimeout(SpeedDown, 3f);
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

        private bool IsDoubleJumpAllowed()
        {
            if (_inventory != null && _inventory.Has(PlayerData.DoubleJumper)) return true;
            
            return _perksModel != null && _perksModel.IsUsed(PerksModel.PerkDoubleJump);
        }

        private bool IsInfiniteJumpAllowed()
        {
            return _inventory?.Has(PlayerData.InfiniteJumper) ?? false;
        }

        private float CalculateSpeed()
        {
            return speed * _speedingC;
        }

        public override void Die()
        {
            SetDirection(0);
            _rb.velocity = Vector2.zero;
            
            onJumpStarted = delegate {};
            onDoubleJumpStarted = delegate {};
            onGrounded = delegate {};
            onLongFallGrounded = delegate {};
            
            base.Die();
        }
    }
}