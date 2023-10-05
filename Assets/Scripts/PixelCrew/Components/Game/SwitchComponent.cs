using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(Animator))]
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private string key = null;
        [SerializeField] private bool state;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private UnityEvent onClose;

        public void Switch()
        {
            state = !state;
            if (animator != null && key != null) animator.SetBool(key, state);
        }

        private void TriggerAnimationFinish()
        {
            if (state) onOpen?.Invoke();
        }

        private void TriggerAnimationStart()
        {
            if (!state) onClose?.Invoke();
        }

        public bool IsSwitched()
        {
            return state;
        }

        public void SetSwitched(bool v)
        {
            if (v != state) Switch();
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}