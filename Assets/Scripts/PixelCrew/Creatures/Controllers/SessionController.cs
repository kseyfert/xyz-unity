using PixelCrew.Components.Singletons;
using PixelCrew.Creatures.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Controllers
{
    public class SessionController : AController
    {
        private GameSessionSingleton _gameSession;

        private void Awake()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
        }

        public PlayerData GetModel()
        {
            return _gameSession.Model;
        }
        
        public override void Die()
        {
        }
    }
}