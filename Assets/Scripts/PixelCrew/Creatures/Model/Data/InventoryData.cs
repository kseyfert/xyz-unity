using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Creatures.Model.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Creatures.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> items = new List<InventoryItemData>();

        public int Add(string id, int value)
        {
            Debug.Log($"IADD: {id}-{value}");
            if (!DefsFacade.I.IsExist(id)) return 0;
            if (value <= 0) return 0;

            var item = GetOrCreateItem(id);
            item.value += value;

            return value;
        }

        public int Remove(string id, int value)
        {
            Debug.Log($"IREM {id}-{value}");
            if (!DefsFacade.I.IsExist(id)) return 0;
            if (value <= 0) return 0;
            
            var item = GetItem(id);
            if (item == null) return 0;

            if (item.value <= value)
            {
                Debug.Log($"COUNT: {items.Count}");
                items.Remove(item);
                Debug.Log($"COUNT: {items.Count}");
                return item.value;
            }

            item.value -= value;

            return value;
        }

        public int Apply(string id, int value)
        {
            return value >= 0 ? Add(id, value) : Remove(id, -value);
        }

        public void RemoveAll(string id)
        {
            if (!DefsFacade.I.IsExist(id)) return;
            
            var item = GetItem(id);
            if (item == null) return;

            items.Remove(item);
        }

        public bool Has(string id)
        {
            return Count(id) > 0;
        }

        public int Count(string id)
        {
            return items
                .FindAll(item => item.id == id)
                .Sum(item => item.value);
        }

        private InventoryItemData GetOrCreateItem(string id)
        {
            var found = items.Find(item => item.id == id);
            if (found != null) return found;

            var newItem = new InventoryItemData(id);
            items.Add(newItem);

            return newItem;
        }

        private InventoryItemData GetItem(string id)
        {
            return items.Find(item => item.id == id);
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        public string id;
        public int value;

        public InventoryItemData(string id)
        {
            this.id = id;
            this.value = 0;
        }

        public InventoryItemData(string id, int value)
        {
            this.id = id;
            this.value = value;
        }
    }
}