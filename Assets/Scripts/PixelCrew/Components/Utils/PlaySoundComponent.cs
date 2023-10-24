using System;
using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class PlaySoundComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioData[] sounds;

        public void Play(string id)
        {
            var sound = Array.Find(sounds, item => item.Id == id);
            if (sound == null) return;
            
            source.PlayOneShot(sound.Clip);
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