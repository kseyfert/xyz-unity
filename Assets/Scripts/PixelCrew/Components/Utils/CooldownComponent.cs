using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils
{
    public class CooldownComponent : MonoBehaviour
    {
        [SerializeField] private Cooldown cooldown;
        [SerializeField] private UnityEvent action;
        
        public void Trigger()
        {
            if (!cooldown.IsReady) return;
            action?.Invoke();
            cooldown.Reset();
        }
    }
}