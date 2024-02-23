using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef items;
        [SerializeField] private ThrowableItemsDef throwable;
        [SerializeField] private PlayerDef player;
        [SerializeField] private PotionsDef potions;

        [SerializeField] private PerksRepository perks;

        public InventoryItemsDef Items => items;
        public ThrowableItemsDef Throwable => throwable;
        public PlayerDef Player => player;
        public PotionsDef Potions => potions;
        public PerksRepository Perks => perks;

        public bool IsExist(string id)
        {
            var def = items.Get(id);
            return !def.IsVoid;
        }
        
        private static DefsFacade _instance;
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            _instance = Resources.Load<DefsFacade>("DefsFacade");
            return _instance;
        }
    }
}