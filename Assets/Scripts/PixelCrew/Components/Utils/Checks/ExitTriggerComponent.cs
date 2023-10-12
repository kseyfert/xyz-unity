using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils.Checks
{
    [RequireComponent(typeof(Collider2D))]
    public class ExitTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private TriggerEvent action;

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(layerMask)) return;
            if (!string.IsNullOrEmpty(targetTag) && !other.gameObject.CompareTag(targetTag)) return;
         
            action?.Invoke(other.gameObject);
        }

        [Serializable]
        private class TriggerEvent : UnityEvent<GameObject> {}
    }
}