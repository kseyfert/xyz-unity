using System;
using PixelCrew.Components;
using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model;
using PixelCrew.Creatures.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Creatures.Controllers
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthController : AController
    {
        public delegate void HealthDelegate();

        public HealthDelegate onDamage;
        public HealthDelegate onHeal;
        public HealthDelegate onDie;
        
        [SerializeField] private int potionPower;
        [SerializeField] private ProgressBarWidget widget;

        private SessionController _sessionController;
        private HealthComponent _healthComponent;
        private InventoryData _inventory;

        private void Start()
        {
            _sessionController = Creature.SessionController;
            _healthComponent = GetComponent<HealthComponent>();
            
            if (_sessionController != null) _inventory = _sessionController.GetModel().inventory;
            
            _healthComponent.onChange += SaveToSession;
            
            LoadFromSession();
            if (_healthComponent.GetCurrentHealth() == 0) _healthComponent.SetCurrentHealth(_healthComponent.GetMaxHealth());

            _healthComponent.onDamage.AddListener(() => onDamage?.Invoke());
            _healthComponent.onHeal.AddListener(() => onHeal?.Invoke());
            _healthComponent.onDie.AddListener(() => onDie?.Invoke());

            if (widget != null)
            {
                _healthComponent.onDamage.AddListener(UpdateWidget);
                _healthComponent.onHeal.AddListener(UpdateWidget);
            }
        }

        private void UpdateWidget()
        {
            var value = (float) _healthComponent.GetCurrentHealth() / _healthComponent.GetMaxHealth();
            widget.SetProgress(value);
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

            _healthComponent.SetCurrentHealth(_sessionController.GetModel().hp.Value);
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;

            _sessionController.GetModel().hp.Value = _healthComponent.GetCurrentHealth();
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