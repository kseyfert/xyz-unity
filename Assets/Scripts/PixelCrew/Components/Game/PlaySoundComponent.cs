using System;
using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class PlaySoundComponent : MonoBehaviour
    {
        [SerializeField] private AudioData[] sounds;

        private AudioSource _source;

        private void Start()
        {
            _source = SingletonMonoBehaviour.GetInstance<SfxSingleton>().Source;
        }

        public void Play(string id)
        {
            var sound = Array.Find(sounds, item => item.Id == id);
            if (sound == null) return;
            
            _source.PlayOneShot(sound.Clip);
        }

        [Serializable]
        public class AudioData
        {
            [SerializeField] private string id;
            [SerializeField] private AudioClip clip;

            public string Id => id;
            public AudioClip Clip => clip;
        }

    }
}