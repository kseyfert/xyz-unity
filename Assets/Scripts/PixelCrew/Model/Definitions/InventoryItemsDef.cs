using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
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
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemTag[] tags;
        
        public string Id => id;
        public Sprite Icon => icon;
        public bool IsVoid => string.IsNullOrEmpty(id);

        public bool HasTag(ItemTag tag)
        {
            return tags != null && tags.Contains(tag);
        }
    }
}