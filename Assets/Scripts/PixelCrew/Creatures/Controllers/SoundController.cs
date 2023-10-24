using PixelCrew.Components.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(PlaySoundComponent))]
    public class SoundController : AController
    {
        private PlaySoundComponent _playSoundComponent;

        private void Start()
        {
            _playSoundComponent = GetComponent<PlaySoundComponent>();
        }

        public void Play(string id)
        {
            _playSoundComponent.Play(id);
        }
    }
}