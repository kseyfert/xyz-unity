using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    [CreateAssetMenu(menuName = "Defs/PerksDef", fileName = "PerksDef")]
    public class PerksRepository : DefRepository<PerkDef>
    {
        
    }

    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private string info;
        [SerializeField] private ItemWithCount price;

        public string Id => id;
        public Sprite Icon => icon;
        public string Info => info;
        public ItemWithCount Price => price;
    }

    [Serializable]
    public struct ItemWithCount
    {
        [InventoryId]
        [SerializeField] private string id;
        [SerializeField] private int count;

        public string Id => id;
        public int Count => count;
    }
}