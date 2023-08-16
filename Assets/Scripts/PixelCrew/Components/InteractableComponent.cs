using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent action;
        [SerializeField] Animator animator;
        [SerializeField] private string animatorTrigger;
        [SerializeField] private bool active = true;

        public void Interact()
        {
            if (!active) return;
            
            action?.Invoke();
            if (animator != null && animatorTrigger != null) animator.SetTrigger(animatorTrigger);            
        }

        public void Disable()
        {
            active = false;
        }

        public void Enable()
        {
            active = true;
        }
    }
}