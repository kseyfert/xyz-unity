using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils
{
    public class AwaitComponent : MonoBehaviour
    {
        [SerializeField] private float timeout;
        [SerializeField] private UnityEvent action;
        [SerializeField] private bool startImmediate = false;
        [SerializeField] private float randomizedAmplitude = 0f;

        private Cooldown _cooldown = new Cooldown();
        private bool _started;

        private void Start()
        {
            if (startImmediate) StartWaiting();
        }

        public void StartWaiting()
        {
            _started = true;

            var shift = randomizedAmplitude * Random.value - (randomizedAmplitude / 2f);
            _cooldown.Reset(timeout + shift);
        }

        private void Update()
        {
            if (!_started) return;
            if (!_cooldown.IsReady) return;
            
            _started = false;
            
            action?.Invoke();
        }

    }
}