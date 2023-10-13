using System;
using PixelCrew.Components.Utils;
using PixelCrew.Creatures.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(UniqueIDComponent))]
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
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

        [SerializeField] private DieEvent onDie;

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
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;
        
        public Transform Transform => _transform;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public Animator Animator => _animator;
        public Collider2D Collider2D => _collider2D;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        private void Awake()
        {
            ID = GetComponent<UniqueIDComponent>().GetID();

            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
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
                
                attackController.OnThrowStarted += (obj, args) => animationController.SetTrigger(AnimationController.TriggerThrow);
                attackController.OnThrowFinished += (obj, args) => particlesController.Spawn("sword-thrown");
                
                attackController.OnThrowMaxStarted += (obj, args) => animationController.SetTrigger(AnimationController.TriggerThrowMax);
                attackController.OnThrowMaxFinished += (obj, args) =>
                {
                    var a = (AttackController.ThrowMaxEventArgs)args;
                    particlesController.SpawnSeq("sword-thrown", a.count, a.timeout);
                };

                attackController.OnArm += (obj, args) => animationController.SetProfile("armed");
                attackController.OnUnarm += (obj, args) => animationController.SetProfile("unarmed");
                
                if (attackController.IsArmed()) animationController.SetProfile("armed");
                else animationController.SetProfile("unarmed");
            }

            if (animationController != null)
            {
                animationController.SetBoolUpdate(AnimationController.BoolIsGrounded, () => movementController == null || movementController.IsGrounded());
                animationController.SetBoolUpdate(AnimationController.BoolIsRunning, () => Math.Abs(_rigidbody2D.velocity.x) > 0.01f);
                animationController.SetBoolUpdate(AnimationController.BoolIsDoubleJumping, () => movementController != null && movementController.IsDoubleJumping());
                
                animationController.SetFloatUpdate(AnimationController.FloatVelocityY, () => _rigidbody2D.velocity.y);
            }

            if (healthController != null && animationController != null)
            {
                healthController.OnDamage +=
                    (obj, args) =>
                    {
                        animationController.SetTrigger(AnimationController.TriggerHit);
                        if (movementController != null) movementController.Kickback();
                    };
                healthController.OnDie += (obj, args) => animationController.SetTrigger(AnimationController.TriggerHit);
                
                animationController.SetBoolUpdate(AnimationController.BoolIsDead, () => healthController.GetHealthComponent().IsDead());
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

        public void Throw()
        {
            if (attackController == null) return;
            
            attackController.DoThrow();
        }

        public void ThrowMax()
        {
            if (attackController == null) return;
            
            attackController.DoThrowMax();
        }

        public void Die()
        {
            if (animationController != null) animationController.Die();
            if (attackController != null) attackController.Die();
            if (coinsController != null) coinsController.Die();
            if (healthController != null) healthController.Die();
            if (interactionController != null) interactionController.Die();
            if (movementController != null) movementController.Die();
            if (particlesController != null) particlesController.Die();
            if (sessionController != null) sessionController.Die();
                    
            if (_collider2D != null) _collider2D.enabled = false;
            if (_spriteRenderer != null) _spriteRenderer.enabled = false;
                    
            if (particlesController != null) particlesController.Spawn("dead");
            
            onDie?.Invoke(gameObject);
        }
        
        [Serializable]
        private class DieEvent : UnityEvent<GameObject> {}
    }
}