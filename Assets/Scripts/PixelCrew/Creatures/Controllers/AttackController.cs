using System;
using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class AttackController : AController
    {
        public event EventHandler OnAttackStarted;
        public event EventHandler OnArm;
        public event EventHandler OnUnarm;
        public event EventHandler OnThrowStarted;
        public event EventHandler OnThrowFinished;
        public event EventHandler OnThrowMaxStarted;
        public event EventHandler OnThrowMaxFinished;

        [SerializeField] private Creature creature;

        [SerializeField] private CircleOverlapCheckComponent attackPosition;
        [SerializeField] private int damagePower = 10;
        [SerializeField] private bool armed;
        [SerializeField] private Cooldown cooldown;
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

        public void Throw()
        {
            if (!armed) return;
            if (!cooldown.IsReady) return;
            if (_currentStock == 1) return;

            _currentStock--;
            Debug.Log($"Current Stock: {_currentStock}");

            cooldown.Reset();
            OnThrowStarted?.Invoke(this, EventArgs.Empty);
        }

        public void DoThrow()
        {
            if (!armed) return;
            
            OnThrowFinished?.Invoke(this, EventArgs.Empty);
        }
        
        public void ThrowMax()
        {
            if (!armed) return;
            if (!cooldown.IsReady) return;
            if (_currentStock <= 3)
            {
                Throw();
                return;
            };

            _currentStock -= 3;
            Debug.Log($"Current Stock: {_currentStock}");

            cooldown.Reset();
            OnThrowMaxStarted?.Invoke(this, EventArgs.Empty);
        }
        
        public void DoThrowMax()
        {
            if (!armed) return;
            
            var args = new ThrowMaxEventArgs
            {
                count = throwMaxCount,
                timeout = throwMaxTimeout
            };

            OnThrowMaxFinished?.Invoke(this, args);
        }

        public void Arm(int stock = 1)
        {
            stock = Math.Max(stock, 1);
            _currentStock += stock;
            armed = true;
            
            SaveToSession();
            
            OnArm?.Invoke(this, EventArgs.Empty);
            
            Debug.Log($"Current Stock: {_currentStock}");
        }

        public void Unarm()
        {
            _currentStock = 0;
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

        public class ThrowMaxEventArgs : EventArgs
        {
            public int count;
            public float timeout;
        }
    }
}