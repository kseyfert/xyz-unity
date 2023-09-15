using System;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;
        [SerializeField] private UnityEvent onDamage;
        [SerializeField] private UnityEvent onHeal;
        [SerializeField] private UnityEvent onDie;

        private GameSession _gameSession = null;
        
        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void LinkGameSession(GameSession gameSession)
        {
            _gameSession = gameSession;
            LoadFromSession();
        }

        private void LoadFromSession()
        {
            if (_gameSession == null) return;

            currentHealth = _gameSession.Data.hp;
        }

        private void SaveToSession()
        {
            if (_gameSession == null) return;

            _gameSession.Data.hp = currentHealth;
        }

        public void ApplyDamage(int value)
        {
            if (value <= 0) return;
            Add(-value);
        }

        public void ApplyHeal(int value)
        {
            if (value <= 0) return;
            Add(value);
        }

        public void Add(int value)
        {
            currentHealth += value;
            currentHealth = Math.Min(currentHealth, maxHealth);
            currentHealth = Math.Max(currentHealth, 0);
            
            SaveToSession();

            if (currentHealth == 0)
            {
                onDie?.Invoke();
                return;
            }
            
            if (value > 0) onHeal?.Invoke();
            if (value < 0) onDamage?.Invoke();
        }
    }
}