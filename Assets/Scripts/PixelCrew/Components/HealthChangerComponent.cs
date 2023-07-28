using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthChangerComponent : MonoBehaviour
    {
        [SerializeField] private int value;
        [SerializeField] private UnityEvent onApply;

        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent == null) return;
            
            healthComponent.Add(value);
            onApply?.Invoke();
        }
    }
}