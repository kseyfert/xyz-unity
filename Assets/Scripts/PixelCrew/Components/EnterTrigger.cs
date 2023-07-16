using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private UnityEvent action;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                action?.Invoke();
            }
        }
    }
}