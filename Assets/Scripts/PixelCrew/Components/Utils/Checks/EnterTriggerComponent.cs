using System;
using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Utils;

namespace PixelCrew.Components.Utils.Checks
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private LayerMask layerMask = ~0;
        [SerializeField] private TriggerEvent action;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(layerMask)) return;
            if (!string.IsNullOrEmpty(targetTag) && !other.gameObject.CompareTag(targetTag)) return;
            
            action?.Invoke(other.gameObject);
        }

        [Serializable]
        private class TriggerEvent : UnityEvent<GameObject> {}
    }
}