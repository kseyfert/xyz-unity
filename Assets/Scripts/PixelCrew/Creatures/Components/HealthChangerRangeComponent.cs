using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Components
{
    public class HealthChangerRangeComponent : MonoBehaviour
    {
        [SerializeField] private CircleOverlapCheckComponent creaturesChecker;
        [SerializeField] private int value;
        [SerializeField] private string targetTags;
        [SerializeField] private UnityEvent onApply;

        public void ApplyAll()
        {
            if (creaturesChecker == null) return;

            var applied = false;
            var gos = creaturesChecker.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                if (!item.CompareTags(targetTags)) continue;
                
                var hc = GetHealthComponent(item);
                if (hc == null) return;
            
                hc.Add(value);
                applied = true;
            }
            
            if (applied) onApply?.Invoke();
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