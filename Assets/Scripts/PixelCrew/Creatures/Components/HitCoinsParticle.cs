using PixelCrew.Components.Game;
using PixelCrew.Creatures.Controllers;
using PixelCrew.Creatures.Model;
using UnityEngine;

namespace PixelCrew.Creatures.Components
{
    public class HitCoinsParticle : ReplaceableParticleSystemComponent
    {
        [SerializeField] private Creature creature;

        private CoinsController _coinsController;

        private void Start()
        {
            if (creature == null) return;
            _coinsController = creature.CoinsController;
        }

        public override void RunBurst(int count)
        {
            if (_coinsController == null) return;
            var actualCount = _coinsController.SubAmount(count);
            base.RunBurst(actualCount);
        }
    }
}