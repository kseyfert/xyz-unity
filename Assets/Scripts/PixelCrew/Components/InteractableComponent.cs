using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent action;
        [SerializeField] Animator animator;
        [SerializeField] private string animatorTrigger;

        public void Interact()
        {
            action?.Invoke();
            if (animator != null && animatorTrigger != null) animator.SetTrigger(animatorTrigger);            
        }
    }
}