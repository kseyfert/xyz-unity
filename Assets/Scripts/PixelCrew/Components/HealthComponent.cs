using System;
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

        private void Awake()
        {
            currentHealth = maxHealth;
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