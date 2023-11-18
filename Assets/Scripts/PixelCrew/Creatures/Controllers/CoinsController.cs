using PixelCrew.Model.Data;

namespace PixelCrew.Creatures.Controllers
{
    public class CoinsController : AController
    {
        private InventoryData _inventory;

        private void Start()
        {
            var sessionController = Creature.SessionController;
            if (sessionController == null) return;
            
            _inventory = sessionController.GetModel().inventory;
        }
        
        private int ApplyToInventory(int value)
        {
            return _inventory?.Apply(PlayerData.Coins, value) ?? 0;
        }

        public int AddAmount(int value)
        {
            return ApplyAmount(value);
        }

        public int SubAmount(int value)
        {
            return ApplyAmount(-value);
        }

        public int ApplyAmount(int value)
        {
            return ApplyToInventory(value);
        }

        public int GetAmount()
        {
            return _inventory.Count(PlayerData.Coins);
        }
    }
}