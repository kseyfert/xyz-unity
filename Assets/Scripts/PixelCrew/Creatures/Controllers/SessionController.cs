using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class SessionController : AController
    {
        [SerializeField] private Creature creature;
        
        private GameSessionComponent _gameSessionComponent;

        private void OnEnable()
        {
            _gameSessionComponent = FindObjectOfType<GameSessionComponent>();
        }

        public CreatureModel GetModel()
        {
            return _gameSessionComponent.GetCreatureModel(creature.ID);
        }

        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
        }
    }
}