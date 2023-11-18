using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Singletons
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxSingleton : SingletonMonoBehaviour
    {
        private AudioSource _source;
        public AudioSource Source => _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Load<SfxSingleton>();
        }
    }
}