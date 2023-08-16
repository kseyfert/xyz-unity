using System;
using PixelCrew.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HealthComponent))]
    public class Hero : MonoBehaviour
    {
        private static readonly int KeyIsGrounded = Animator.StringToHash("is-grounded");
        private static readonly int KeyIsRunning = Animator.StringToHash("is-running");
        private static readonly int KeyVelocityY = Animator.StringToHash("velocity-y");
        private static readonly int KeyIsDoubleJumping = Animator.StringToHash("is-double-jumping");
        private static readonly int KeyHit = Animator.StringToHash("hit");

        [SerializeField] private SpawnComponent runDustSpawn;
        [SerializeField] private SpawnComponent jumpDustSpawn;
        [SerializeField] private SpawnComponent fallDustSpawn;

        private MovementController _movementController;
        private Rigidbody2D _rb;
        private Animator _animator;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _movementController = GetComponentInChildren<MovementController>();
        }

        private void FixedUpdate()
        {
            var isGrounded = _movementController.IsGrounded();
            var isJumping = _movementController.IsJumping();
            var isDoubleJumping = _movementController.IsDoubleJumping();
            
            _animator.SetBool(KeyIsGrounded, isGrounded);
            _animator.SetBool(KeyIsRunning, Math.Abs(_rb.velocity.x) > 0.01f);
            _animator.SetFloat(KeyVelocityY, _rb.velocity.y);
            _animator.SetBool(KeyIsDoubleJumping, isDoubleJumping);
        }

        public void OnDamage()
        {
            _animator.SetTrigger(KeyHit);
            _movementController.Kickback();
        }

        public void SpawnRunDust()
        {
            runDustSpawn.Spawn();
        }

        public void SpawnJumpDust()
        {
            jumpDustSpawn.Spawn();
        }

        public void SpawnFallDust()
        {
            fallDustSpawn.Spawn();
        }

        public void Freeze()
        {
            _movementController.Freeze(100);
        }
        public void Unfreeze()
        {
            _movementController.Unfreeze();
        }
    }
}