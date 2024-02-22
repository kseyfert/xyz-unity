using PixelCrew.Components.Singletons;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class SessionController : AController
    {
        private GameSessionSingleton _gameSession;

        public PlayerData GetModel()
        {
            return GetGameSession().Model;
        }

        public QuickInventoryModel GetQuickInventory()
        {
            return GetGameSession().QuickInventoryModel;
        }

        public PerksModel GetPerksModel()
        {
            return GetGameSession().PerksModel;
        }

        private GameSessionSingleton GetGameSession()
        {
            if (_gameSession != null) return _gameSession;
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            return _gameSession;
        }
        
        public override void Die()
        {
        }
    }
}