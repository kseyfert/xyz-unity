using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
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