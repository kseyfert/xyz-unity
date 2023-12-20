using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Singletons
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxSingleton : SingletonMonoBehaviour
    {
        private AudioSource _source;
        public AudioSource Source => _source;

        protected override void Awake()
        {
            base.Awake();
            
            _source = GetComponent<AudioSource>();
        }
    }
}