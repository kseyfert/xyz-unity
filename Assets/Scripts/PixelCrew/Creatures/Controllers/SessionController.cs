using PixelCrew.Components.Game;
using PixelCrew.Components.Singletons;
using PixelCrew.Creatures.Model;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class SessionController : AController
    {
        private GameSessionSingleton _gameSession;

        private void Awake()
        {
            _gameSession = GameSessionSingleton.GetInstance();
        }

        public CreatureModel GetModel()
        {
            return _gameSession.GetCreatureModel(Creature.ID);
        }
        
        public override void Die()
        {
        }
    }
}