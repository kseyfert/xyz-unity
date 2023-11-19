using PixelCrew.Components.Singletons;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Controllers
{
    public class UIController : AController
    {
        private GameSessionSingleton _gameSession;

        private void Start()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
        }

        public void QuickInventoryNextItem()
        {
            _gameSession.QuickInventoryModel.SetNextItem();
        }
    }
}