using System;
using UnityEngine;

namespace PixelCrew.Creatures.Model.Definitions
{
    [Serializable]
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] items;

        public ItemDef Get(string id)
        {
            foreach (var item in items)
            {
                if (item.Id == id) return item;
            }

            return default;
        }

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => items;
#endif

    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string id;
        [SerializeField] private bool stackable;
        [SerializeField] private Sprite icon;
        public string Id => id;
        public bool Stackable => stackable;
        public Sprite Icon => icon;
        public bool IsVoid => string.IsNullOrEmpty(id);
    }
}