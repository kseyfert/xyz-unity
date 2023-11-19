using PixelCrew.Components.Singletons;
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

        public QuickInventoryModel GetQuickInventory()
        {
            return _gameSession.QuickInventoryModel;
        }
        
        public override void Die()
        {
        }
    }
}