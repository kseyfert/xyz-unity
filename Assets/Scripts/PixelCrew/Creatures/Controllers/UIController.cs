using PixelCrew.Components.Singletons;
using PixelCrew.Model.Definitions;
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

        public void Use()
        {
            if (_gameSession.QuickInventoryModel.SelectedItemDef.HasTag(ItemTag.Throwable)) Creature.AttackController.RequestRange();
            else
            {
                Creature.HealthController.ApplyPotion();
                Creature.MovementController.ApplyPotion();
            }
        }

        public void UseMax()
        {
            if (_gameSession.QuickInventoryModel.SelectedItemDef.HasTag(ItemTag.Throwable)) Creature.AttackController.RequestRangeMax();
            else
            {
                Creature.HealthController.ApplyPotion();
                Creature.MovementController.ApplyPotion();
            }
        }
    }
}