using System;
using PixelCrew.Components;
using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model;
using PixelCrew.Creatures.Model.Data;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthController : AController
    {
        public delegate void HealthDelegate();

        public HealthDelegate onDamage;
        public HealthDelegate onHeal;
        public HealthDelegate onDie;
        
        [SerializeField] private Creature creature;
        [SerializeField] private int potionPower;

        private SessionController _sessionController;
        private HealthComponent _healthComponent;
        private InventoryData _inventory;

        private void Start()
        {
            _sessionController = creature.SessionController;
            _healthComponent = GetComponent<HealthComponent>();

            if (_sessionController != null) _inventory = _sessionController.GetModel().inventory;
            
            _healthComponent.onChange += SaveToSession;
            LoadFromSession();

            _healthComponent.onDamage.AddListener(() => onDamage?.Invoke());
            _healthComponent.onHeal.AddListener(() => onHeal?.Invoke());
            _healthComponent.onDie.AddListener(() => onDie?.Invoke());
        }

        public void ApplyPotion()
        {
            if (_inventory == null) return;
            if (!_inventory.Has(CreatureModel.Potions)) return;

            _inventory.Remove(CreatureModel.Potions, 1);
            _healthComponent.ApplyHeal(potionPower);
        }

        public HealthComponent GetHealthComponent()
        {
            return _healthComponent;
        }

        private void LoadFromSession()
        {
            if (_sessionController == null) return;

            _healthComponent.SetCurrentHealth(_sessionController.GetModel().hp);
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;

            _sessionController.GetModel().hp = _healthComponent.GetCurrentHealth();
        }

        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
            onDamage = delegate {};
            onHeal = delegate {};
            onDie = delegate {};
            
            base.Die();
        }
    }
}