using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Components
{
    public class CoinsChangerComponent : MonoBehaviour
    {
        [SerializeField] private int value;
        [SerializeField] private UnityEvent onApply;

        public void Apply(GameObject target)
        {
            var creature = target.GetComponent<Creature>();
            if (creature == null) return;

            var coinsController = creature.CoinsController;
            if (coinsController == null) return;
            
            coinsController.ApplyAmount(value);
            onApply?.Invoke();
        }
    }
}