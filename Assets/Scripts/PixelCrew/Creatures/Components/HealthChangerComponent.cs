using PixelCrew.Components.Game;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Components
{
    public class HealthChangerComponent : MonoBehaviour
    {
        [SerializeField] private int value;
        [SerializeField] private UnityEvent onApply;

        public void Apply(GameObject target)
        {
            var hc = GetHealthComponent(target);
            if (hc == null) return;
            
            hc.Add(value);
            onApply?.Invoke();
        }

        private HealthComponent GetHealthComponent(GameObject target)
        {
            var creature = target.GetComponent<Creature>();
            if (creature == null) return target.GetComponent<HealthComponent>();
            
            var healthController = creature.HealthController;
            if (healthController == null) return null;

            return healthController.GetHealthComponent();
        }
    }
}