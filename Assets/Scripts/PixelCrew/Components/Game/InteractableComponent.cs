using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private InteractionEvent action;
        [SerializeField] Animator animator;
        [SerializeField] private string animatorTrigger;
        [SerializeField] private bool active = true;

        public void Interact(GameObject go)
        {
            if (!active) return;
            
            action?.Invoke(go);
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

        [Serializable]
        private class InteractionEvent : UnityEvent<GameObject> {}
    }
}