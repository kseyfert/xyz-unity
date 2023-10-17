using System;
using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class AttackController : AController
    {
        public delegate void AttackDelegate();

        public AttackDelegate onAttackStarted;
        public AttackDelegate onArm;
        public AttackDelegate onUnarm;
        public AttackDelegate onThrowStarted;
        public AttackDelegate onThrowFinished;
        public AttackDelegate onThrowMaxStarted;

        public delegate void AttackThrowMaxDelegate(int count, float timeout);
        public AttackThrowMaxDelegate onThrowMaxFinished;
        
        [SerializeField] private Creature creature;

        [SerializeField] private CircleOverlapCheckComponent attackPosition;
        [SerializeField] private int damagePower = 10;
        [SerializeField] private bool armed;
        [SerializeField] private Cooldown cooldown;
        [SerializeField] private bool infiniteStock = false;
        [SerializeField] private int throwMaxCount = 3;
        [SerializeField] private float throwMaxTimeout = 0.2f;
        
        private int _currentStock;
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
            var stock = _sessionController.GetModel().currentStock;
            
            if (isArmed) Arm(stock);
            else Unarm();
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;

            _sessionController.GetModel().isArmed = armed;
            _sessionController.GetModel().currentStock = _currentStock;
        }

        public void Attack()
        {
            if (!armed) return;
            if (!cooldown.IsReady) return;

            cooldown.Reset();
            onAttackStarted?.Invoke();
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

        public void Throw()
        {
            if (!armed) return;
            if (!cooldown.IsReady) return;
            if (!infiniteStock && _currentStock <= 1) return;

            _currentStock--;
            Debug.Log($"Current Stock: {_currentStock}");

            cooldown.Reset();
            onThrowStarted?.Invoke();
        }

        public void DoThrow()
        {
            if (!armed) return;
            
            onThrowFinished?.Invoke();
        }
        
        public void ThrowMax()
        {
            if (!armed) return;
            if (!cooldown.IsReady) return;
            if (!infiniteStock && _currentStock <= 3)
            {
                Throw();
                return;
            };

            _currentStock -= 3;
            Debug.Log($"Current Stock: {_currentStock}");

            cooldown.Reset();
            onThrowMaxStarted?.Invoke();
        }
        
        public void DoThrowMax()
        {
            if (!armed) return;
            
            onThrowMaxFinished?.Invoke(throwMaxCount, throwMaxTimeout);
        }

        public void Arm(int stock = 1)
        {
            stock = Math.Max(stock, 1);
            _currentStock += stock;
            armed = true;
            
            SaveToSession();
            
            onArm?.Invoke();
            
            Debug.Log($"Current Stock: {_currentStock}");
        }

        public void Unarm()
        {
            _currentStock = 0;
            armed = false;
            
            SaveToSession();
            
            onUnarm?.Invoke();
        }

        public bool IsArmed()
        {
            return armed;
        }

        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
            onAttackStarted = delegate () {};
            onArm = delegate () {};
            onUnarm = delegate () {};
            onThrowStarted = delegate () {};
            onThrowFinished = delegate () {};
            onThrowMaxStarted = delegate () {};
            onThrowMaxFinished = delegate(int _, float __) {};
            
            base.Die();
        } 
    }
}