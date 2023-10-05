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
            var creature = target.GetComponent<Creature>();
            if (creature == null) return;

            var healthController = creature.HealthController;
            if (healthController == null) return;
            
            healthController.GetHealthComponent().Add(value);
            onApply?.Invoke();
        }
    }
}