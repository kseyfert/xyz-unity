using System;
using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class FrozenScaleComponent : MonoBehaviour
    {
        [SerializeField] private bool x;
        [SerializeField] private bool y;
        [SerializeField] private bool z;

        private Vector3 _initialLossy;

        private void Start()
        {
            _initialLossy = transform.lossyScale;
        }

        private void Update()
        {
            if (!x && !y && !z) return;
            if (transform.lossyScale.Equals(_initialLossy)) return;

            var parent = transform.parent;
            transform.parent = null;
            transform.localScale = _initialLossy;
            transform.parent = parent;
        }
    }
}