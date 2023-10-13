using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils
{
    public class AwaitComponent : MonoBehaviour
    {
        [SerializeField] private float timeout;
        [SerializeField] private UnityEvent action;

        private Cooldown _cooldown = new Cooldown();
        private bool _started;

        public void StartWaiting()
        {
            _started = true;
            _cooldown.Reset(timeout);
        }

        private void Update()
        {
            if (!_started) return;
            if (!_cooldown.IsReady) return;
            
            action?.Invoke();
            enabled = false;
        }

    }
}