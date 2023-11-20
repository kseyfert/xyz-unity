using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef items;
        [SerializeField] private ThrowableItemsDef throwable;
        [SerializeField] private PlayerDef player;
        [SerializeField] private PotionsDef potions;

        public InventoryItemsDef Items => items;
        public ThrowableItemsDef Throwable => throwable;
        public PlayerDef Player => player;
        public PotionsDef Potions => potions;

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