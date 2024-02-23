using System;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data
{
    public class StatsModel : IDisposable
    {
        private readonly PlayerData _data;
        private Action _onChanged = () => { };

        public StatsModel(PlayerData data)
        {
            _data = data;
        }

        public bool LevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            if (def.Levels.Length <= nextLevel) return false;

            var price = def.Levels[nextLevel].Price;
            if (!_data.inventory.Has(price)) return false;

            _data.inventory.Remove(price.Id, price.Count);
            _data.levels.LevelUp(id);
            
            _onChanged?.Invoke();
            return true;
        }

        public bool LevelUp(string id)
        {
            return LevelUp((StatId)Enum.Parse(typeof(StatId), id));
        }

        public bool CanLevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            if (def.Levels.Length <= nextLevel) return false;

            var price = def.Levels[nextLevel].Price;
            return _data.inventory.Has(price);
        }

        public bool CanLevelUp(string id)
        {
            return CanLevelUp((StatId)Enum.Parse(typeof(StatId), id));
        }

        public float GetValue(StatId id)
        {
            return GetCurrentLevelDef(id).Value;
        }

        public int GetCurrentLevel(StatId id) => _data.levels.GetLevel(id);

        public StatLevel GetCurrentLevelDef(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            return def.Levels[GetCurrentLevel(id)];
        }

        public StatLevel GetCurrentLevelDef(string id)
        {
            return GetCurrentLevelDef((StatId) Enum.Parse(typeof(StatId), id));
        }

        public bool HasNextLevel(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            return def.Levels.Length > nextLevel;
        }

        public bool HasNextLevel(string id)
        {
            return HasNextLevel((StatId)Enum.Parse(typeof(StatId), id));
        }

        public StatLevel GetNextLevelDef(StatId id)
        {
            if (!HasNextLevel(id)) return default;
            
            var def = DefsFacade.I.Player.GetStat(id);
            return def.Levels[GetCurrentLevel(id) + 1];
        }

        public StatLevel GetNextLevelDef(string id)
        {
            return GetNextLevelDef((StatId)Enum.Parse(typeof(StatId), id));
        }

        public StatLevel GetLastLevelDef(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            return def.Levels[def.Levels.Length - 1];
        }

        public StatLevel GetLastLevelDef(string id)
        {
            return GetLastLevelDef((StatId)Enum.Parse(typeof(StatId), id));
        }
        
        public ActionDisposable Subscribe(Action call)
        {
            _onChanged += call;
            return new ActionDisposable(() => _onChanged -= call);
        }

        public ActionDisposable SubscribeAndInvoke(Action call)
        {
            call?.Invoke();
            return Subscribe(call);
        }
        
        public void Dispose()
        {
        }
    }
}