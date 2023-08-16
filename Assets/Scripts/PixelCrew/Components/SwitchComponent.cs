using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Animator))]
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private string key = null;
        [SerializeField] private bool state;

        public void Switch()
        {
            state = !state;
            if (animator != null && key != null) animator.SetBool(key, state);
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