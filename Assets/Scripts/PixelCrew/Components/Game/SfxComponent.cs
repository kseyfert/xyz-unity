using System;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxComponent : MonoBehaviour
    {
        private static SfxComponent _instance;
        public static SfxComponent I => _instance == null ? LoadInstance() : _instance;

        private delegate void SfxDelegate();
        private static event SfxDelegate OnLoaded;

        private static SfxComponent LoadInstance()
        {
            _instance = FindObjectOfType<SfxComponent>();
            OnLoaded?.Invoke();
            return _instance;
        }

        private AudioSource _source;
        public AudioSource Source => _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            OnLoaded += OnInstanceLoaded;
            LoadInstance();
        }

        private void OnInstanceLoaded()
        {
            if (this != _instance) gameObject.SetActive(false);

            OnLoaded -= OnInstanceLoaded;
        }

        private void OnDestroy()
        {
            OnLoaded -= OnInstanceLoaded;
        }
    }
}