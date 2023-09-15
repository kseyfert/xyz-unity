using System;
using PixelCrew.Components;
using PixelCrew.Model;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Hero
{
    public class AttackController : MonoBehaviour
    {
        private static readonly int KeyAttack = Animator.StringToHash("attack");

        [SerializeField] private SpawnComponent attackParticles;
        [SerializeField] private CheckCircleOverlap attackPosition;
        [SerializeField] private int damagePower = 10;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimatorController armedController;
        [SerializeField] private AnimatorController unarmedController;
        [SerializeField] private bool armed;

        private GameSession _gameSession;

        public void LinkGameSession(GameSession gameSession)
        {
            _gameSession = gameSession;
            LoadFromSession();
        }

        private void LoadFromSession()
        {
            if (_gameSession == null) return;

            armed = _gameSession.Data.isArmed;
        }

        private void SaveToSession()
        {
            if (_gameSession == null) return;

            _gameSession.Data.isArmed = armed;
        }

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

        public void SpawnParticles()
        {
            if (attackParticles == null) return;
            if (!armed) return;
            
            attackParticles.Spawn();
        }

        public void Arm()
        {
            armed = true;
            SaveToSession();
        }

        public void Unarm()
        {
            armed = false;
            SaveToSession();
        }

        public bool IsArmed()
        {
            return armed;
        }
    }
}