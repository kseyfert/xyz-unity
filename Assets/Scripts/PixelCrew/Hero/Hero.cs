using UnityEngine;

namespace PixelCrew.Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Hero : MonoBehaviour
    {
        private static readonly int KeyIsGrounded = Animator.StringToHash("is-grounded");
        private static readonly int KeyIsRunning = Animator.StringToHash("is-running");
        private static readonly int KeyVelocityY = Animator.StringToHash("velocity-y");
        private static readonly int KeyIsDoubleJumping = Animator.StringToHash("is-double-jumping");

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
            _animator.SetBool(KeyIsRunning, _rb.velocity.x != 0);
            _animator.SetFloat(KeyVelocityY, _rb.velocity.y);
            _animator.SetBool(KeyIsDoubleJumping, isDoubleJumping);
        }
    }
}