using UnityEngine;

namespace PixelCrew.Creatures.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef items;

        public InventoryItemsDef Items => items;

        public bool IsExist(string id)
        {
            var def = items.Get(id);
            return !def.IsVoid;
        }

        public bool IsStackable(string id)
        {
            var def = items.Get(id);
            return def.Stackable;
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