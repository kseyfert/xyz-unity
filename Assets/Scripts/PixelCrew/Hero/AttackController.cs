using System;
using PixelCrew.Components;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Hero
{
    public class AttackController : MonoBehaviour
    {
        private static readonly int KeyAttack = Animator.StringToHash("attack");

        [SerializeField] private CheckCircleOverlap attackPosition;
        [SerializeField] private int damagePower = 10;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorController armedController;
        [SerializeField] private AnimatorController unarmedController;
        [SerializeField] private bool armed;

        private void Update()
        {
            animator.runtimeAnimatorController = armed ? armedController : unarmedController;
        }

        public void OnAttack()
        {
            if (!armed) return;
            
            animator.SetTrigger(KeyAttack);
        }

        public void DoAttack()
        {
            if (!armed) return;

            var gos = attackPosition.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var health = item.GetComponent<HealthComponent>();
                if (health == null) continue;
                
                health.ApplyDamage(damagePower);        
            }
        }

        public void Arm()
        {
            armed = true;
        }

        public void Unarm()
        {
            armed = false;
        }

        public bool IsArmed()
        {
            return armed;
        }
    }
}