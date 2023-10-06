using System;
using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class AttackController : AController
    {
        public event EventHandler OnAttackStarted;
        public event EventHandler OnArm;
        public event EventHandler OnUnarm;

        [SerializeField] private Creature creature;

        [SerializeField] private CircleOverlapCheckComponent attackPosition;
        [SerializeField] private int damagePower = 10;
        [SerializeField] private bool armed;

        private SessionController _sessionController;

        private void Start()
        {
            _sessionController = creature.SessionController;
            
            LoadFromSession();
        }

        private void LoadFromSession()
        {
            if (_sessionController == null) return;

            var isArmed = _sessionController.GetModel().isArmed;
            if (isArmed) Arm();
            else Unarm();
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;

            _sessionController.GetModel().isArmed = armed;
        }

        public void Attack()
        {
            if (!armed) return;

            OnAttackStarted?.Invoke(this, EventArgs.Empty);
        }

        public void DoAttack()
        {
            if (!armed) return;

            var gos = attackPosition.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var otherCreature = item.GetComponent<Creature>();
                if (otherCreature == creature) continue;
                
                var health = item.GetComponent<HealthComponent>();

                if (otherCreature != null)
                {
                    var hc = otherCreature.HealthController;
                    health = hc.GetHealthComponent();
                }
                
                if (health == null) continue;
                
                health.ApplyDamage(damagePower);        
            }
        }

        public void Arm()
        {
            armed = true;
            SaveToSession();
            OnArm?.Invoke(this, EventArgs.Empty);
        }

        public void Unarm()
        {
            armed = false;
            SaveToSession();
            OnUnarm?.Invoke(this, EventArgs.Empty);
        }

        public bool IsArmed()
        {
            return armed;
        }

        protected override Creature GetCreature()
        {
            return creature;
        }
    }
}