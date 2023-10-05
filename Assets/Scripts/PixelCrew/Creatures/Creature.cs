using System;
using PixelCrew.Components;
using PixelCrew.Components.Utils;
using PixelCrew.Creatures.Controllers;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(UniqueIDComponent))]
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Creature : MonoBehaviour
    {
        public string ID { get; private set; }
        
        [SerializeField] private MovementController movementController; 
        [SerializeField] private InteractionController interactionController;
        [SerializeField] private AttackController attackController;
        [SerializeField] private HealthController healthController;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private SessionController sessionController;
        [SerializeField] private ParticlesController particlesController;
        [SerializeField] private CoinsController coinsController;

        public MovementController MovementController => movementController;
        public InteractionController InteractionController => interactionController;
        public AttackController AttackController => attackController;
        public HealthController HealthController => healthController;
        public AnimationController AnimationController => animationController;
        public ParticlesController ParticlesController => particlesController;
        public SessionController SessionController => sessionController;
        public CoinsController CoinsController => coinsController;

        private Transform _transform;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        
        public Transform Transform => _transform;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;

        private void Awake()
        {
            ID = GetComponent<UniqueIDComponent>().GetID();

            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        
            if (particlesController != null && movementController != null)
            {
                movementController.OnJumpStarted += (obj, args) =>
                {
                    if (movementController.IsGrounded()) SpawnParticle("dust-jump");
                };
                movementController.OnLongFallGrounded += (obj, args) => SpawnParticle("dust-fall");
            }

            if (animationController != null && attackController != null)
            {
                attackController.OnAttackStarted += (obj, args) => animationController.SetTrigger(AnimationController.TriggerAttack);

                attackController.OnArm += (obj, args) => animationController.SetProfile("armed");
                attackController.OnUnarm += (obj, args) => animationController.SetProfile("unarmed");
                
                if (attackController.IsArmed()) animationController.SetProfile("armed");
                else animationController.SetProfile("unarmed");
            }

            if (animationController != null)
            {
                animationController.SetBoolUpdate(AnimationController.BoolIsGrounded, () => movementController.IsGrounded());
                animationController.SetBoolUpdate(AnimationController.BoolIsRunning, () => Math.Abs(_rigidbody2D.velocity.x) > 0.01f);
                animationController.SetBoolUpdate(AnimationController.BoolIsDoubleJumping, () => movementController.IsDoubleJumping());
                
                animationController.SetFloatUpdate(AnimationController.FloatVelocityY, () => _rigidbody2D.velocity.y);
            }

            if (healthController != null && animationController != null)
            {
                healthController.OnDamage +=
                    (obj, args) =>
                    {
                        animationController.SetTrigger(AnimationController.TriggerHit);
                        movementController.Kickback();
                    };
            }
        }

        public void SpawnParticle(string particleName)
        {
            if (particlesController == null) return;
            
            particlesController.Spawn(particleName);
        }

        public void Attack()
        {
            if (attackController == null) return;
            
            attackController.DoAttack();
        }
    }
}