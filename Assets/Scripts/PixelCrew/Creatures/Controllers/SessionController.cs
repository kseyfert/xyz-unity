using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class SessionController : AController
    {
        private GameSessionComponent _gameSessionComponent;

        private void Awake()
        {
            _gameSessionComponent = GameSessionComponent.GetInstance();
        }

        public CreatureModel GetModel()
        {
            return _gameSessionComponent.GetCreatureModel(Creature.ID);
        }
        
        public override void Die()
        {
        }
    }
}