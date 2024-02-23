using System;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        public const string PerkDoubleJump = "double-jump";
        public const string PerkRangeMax = "sword-range-max";
        
        private readonly PlayerData _data;

        public PerksModel(PlayerData data)
        {
            _data = data;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnough = _data.inventory.Has(def.Price);

            if (!isEnough) return;

            _data.inventory.Remove(def.Price.Id, def.Price.Count);
            _data.perks.Unlock(id);
        }

        public bool CanUnlock(string id)
        {
            if (IsUnlocked(id)) return false;
            
            var def = DefsFacade.I.Perks.Get(id);
            return _data.inventory.Has(def.Price);
        }

        public void Use(string id)
        {
            _data.perks.Use(id);
        }

        public bool IsUsed(string id)
        {
            return _data.perks.Used.Value == id;
        }

        public bool IsUnlocked(string id)
        {
            return _data.perks.IsUnlocked(id);
        }
        
        public bool IsLocked(string id)
        {
            return _data.perks.IsLocked(id);
        }
        
        public void Dispose()
        {
        }
    }
}