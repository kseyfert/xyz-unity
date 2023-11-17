using System;
using PixelCrew.Components.Utils;
using PixelCrew.Creatures.Controllers;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private SoundController soundController;

        [SerializeField] private DieEvent onDie;

        public MovementController MovementController => movementController;
        public InteractionController InteractionController => interactionController;
        public AttackController AttackController => attackController;
        public HealthController HealthController => healthController;
        public AnimationController AnimationController => animationController;
        public ParticlesController ParticlesController => particlesController;
        public SessionController SessionController => sessionController;
        public CoinsController CoinsController => coinsController;
        public SoundController SoundController => soundController;

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
        }
        
        private void Start() {
            Init();
        }
        
        protected virtual void Init() {
            if (particlesController != null && movementController != null)
            {
                movementController.onJumpStarted += () => particlesController.Spawn("dust-jump");
                movementController.onLongFallGrounded += () => particlesController.Spawn("dust-fall");
            }

            if (animationController != null && attackController != null)
            {
                attackController.onMeleeRequested += () => animationController.SetTrigger(AnimationController.TriggerAttack);
                attackController.onRangeRequested += () => animationController.SetTrigger(AnimationController.TriggerThrow);
                attackController.onRangeMaxRequested += () => animationController.SetTrigger(AnimationController.TriggerThrowMax);

                attackController.onWeaponsChanged += () => animationController.SetProfile(attackController.HasWeapon() ? "armed" : "unarmed");
                animationController.SetProfile(attackController.HasWeapon() ? "armed" : "unarmed");
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
                healthController.onDamage +=
                    () =>
                    {
                        animationController.SetTrigger(AnimationController.TriggerHit);
                        if (movementController != null) movementController.Kickback();
                    };
                healthController.onDie += () => animationController.SetTrigger(AnimationController.TriggerHit);
                
                animationController.SetBoolUpdate(AnimationController.BoolIsDead, () => healthController.GetHealthComponent().IsDead());
            }

            if (soundController != null)
            {
                if (healthController != null)
                {
                    healthController.onDamage += () => soundController.Play("hit");
                    healthController.onDie += () => soundController.Play("die");
                }

                if (attackController != null)
                {
                    attackController.onMeleeRequested += () => soundController.Play("melee");
                    attackController.onRangeRequested += () => soundController.Play("range");
                }

                if (movementController != null)
                {
                    movementController.onJumpStarted += () => soundController.Play("jump");
                    movementController.onDoubleJumpStarted += () => soundController.Play("jump");
                }
            }
        }

        public void SpawnParticle(string particleName)
        {
            if (particlesController == null) return;
            
            particlesController.Spawn(particleName);
        }

        public virtual void AnimationEventMelee()
        {
            if (attackController == null) return;
            
            attackController.DoMelee();
        }

        public virtual void AnimationEventRange()
        {
            if (attackController == null) return;
            
            attackController.DoRange();
        }

        public virtual void AnimationEventRangeMax()
        {
            if (attackController == null) return;
            
            attackController.DoRangeMax();
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