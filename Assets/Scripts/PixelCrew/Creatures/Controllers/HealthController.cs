using System;
using PixelCrew.Components.Game;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
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
        
        [SerializeField] private ProgressBarWidget widget;

        private SessionController _sessionController;
        private HealthComponent _healthComponent;
        private InventoryData _inventory;
        private QuickInventoryModel _quickInventory;

        private void Start()
        {
            _sessionController = Creature.SessionController;
            _healthComponent = GetComponent<HealthComponent>();
            
            if (_sessionController != null) _inventory = _sessionController.GetModel().inventory;
            if (_sessionController != null) _quickInventory = _sessionController.GetQuickInventory();
            
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

            if (_sessionController == null) return;
            
            var statsModel = _sessionController.GetStatsModel();
            statsModel?.SubscribeAndInvoke(() =>
            {
                var maxHealth = _sessionController.GetStatsModel().GetValue(StatId.Hp);
                if (Math.Abs(maxHealth - _healthComponent.GetMaxHealth()) < 0.1f) return;
                
                _healthComponent.SetMaxHealth((int) maxHealth);
                _healthComponent.SetCurrentHealth((int) maxHealth, false);
            });    
        }

        private void UpdateWidget()
        {
            var value = (float) _healthComponent.GetCurrentHealth() / _healthComponent.GetMaxHealth();
            widget.SetProgress(value);
        }

        public void ApplyPotion()
        {
            if (_inventory == null) return;

            var selectedItem = _quickInventory.SelectedItem;

            if (!DefsFacade.I.Potions.Has(selectedItem.id)) return;
            
            var potionDef = DefsFacade.I.Potions.Get(selectedItem.id);
            if (potionDef.Type != PotionType.Health) return;
            
            _inventory.Apply(selectedItem.id, -1);
            _healthComponent.ApplyHeal(potionDef.Power);
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