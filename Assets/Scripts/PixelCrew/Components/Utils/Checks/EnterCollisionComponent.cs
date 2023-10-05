using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils.Checks
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterCollisionComponent : MonoBehaviour 
    {
        [SerializeField] private string targetTag;
        [SerializeField] private CollisionEvent action;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                action?.Invoke(other.gameObject);
            }
        }
        
        [Serializable]
        private class CollisionEvent : UnityEvent<GameObject> {}
    }
}