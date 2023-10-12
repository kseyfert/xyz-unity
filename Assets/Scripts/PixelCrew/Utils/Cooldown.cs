using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private bool enabled = true;
        [SerializeField] private float time;

        private float _timeout;

        public Cooldown()
        {
            time = 0;
        }

        public Cooldown(float t)
        {
            time = t;
            enabled = true;
        }

        public Cooldown(float t, bool e)
        {
            time = t;
            enabled = e;
        }

        public void Reset()
        {
            Reset(time);
        }

        public void Reset(float newTime)
        {
            _timeout = Time.time + newTime;
        }

        public void SetMax(float newTime)
        {
            var newTimeout = Time.time + newTime;
            _timeout = Math.Max(_timeout, newTimeout);
        }

        public bool IsReady => !enabled || _timeout <= Time.time;
    }
}