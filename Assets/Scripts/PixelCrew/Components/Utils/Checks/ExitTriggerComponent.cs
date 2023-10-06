using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils.Checks
{
    [RequireComponent(typeof(Collider2D))]
    public class ExitTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private TriggerEvent action;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                action?.Invoke(other.gameObject);
            }
        }

        [Serializable]
        private class TriggerEvent : UnityEvent<GameObject> {}
    }
}