﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private TriggerEvent action;

        private void OnTriggerEnter2D(Collider2D other)
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