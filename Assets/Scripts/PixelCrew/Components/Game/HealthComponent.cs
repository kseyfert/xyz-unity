using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    public class HealthComponent : MonoBehaviour
    {
        public delegate void HcDelegate();
        public HcDelegate onChange;
        
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;
        [SerializeField] public UnityEvent onDamage;
        [SerializeField] public UnityEvent onHeal;
        [SerializeField] public UnityEvent onDie;

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
            AdjustCurrent();
            
            onChange?.Invoke();

            if (currentHealth == 0)
            {
                onDie?.Invoke();
                return;
            }
            
            if (value > 0) onHeal?.Invoke();
            if (value < 0) onDamage?.Invoke();
        }

        public void SetCurrentHealth(int value)
        {
            currentHealth = value;
            AdjustCurrent();
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        private void AdjustCurrent()
        {
            currentHealth = Math.Min(currentHealth, maxHealth);
            currentHealth = Math.Max(currentHealth, 0);
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}