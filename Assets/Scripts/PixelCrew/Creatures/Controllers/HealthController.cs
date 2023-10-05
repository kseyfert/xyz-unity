using System;
using PixelCrew.Components;
using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthController : MonoBehaviour
    {
        public event EventHandler OnDamage;
        public event EventHandler OnHeal;
        public event EventHandler OnDie;
        
        [SerializeField] private Creature creature;

        private SessionController _sessionController;
        private HealthComponent _healthComponent;

        private void Start()
        {
            _sessionController = creature.SessionController;
            _healthComponent = GetComponent<HealthComponent>();
            
            _healthComponent.OnChange += (obj, args) => SaveToSession();
            LoadFromSession();

            _healthComponent.onDamage.AddListener(() => OnDamage?.Invoke(this, EventArgs.Empty));
            _healthComponent.onHeal.AddListener(() => OnHeal?.Invoke(this, EventArgs.Empty));
            _healthComponent.onDie.AddListener(() => OnDie?.Invoke(this, EventArgs.Empty));
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
    }
}