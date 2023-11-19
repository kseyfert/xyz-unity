using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        private Action<string> _onChange;
        
        [SerializeField] private List<InventoryItemData> items = new List<InventoryItemData>();

        public int Add(string id, int value)
        {
            if (!DefsFacade.I.IsExist(id)) return 0;
            if (value <= 0) return 0;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return 0;
            
            if (itemDef.HasTag(ItemTag.Stackable))
            {
                var item = GetOrCreateItem(id);
                item.value += value;    
            }
            else
            {
                for (var i = 0; i < value; i++) CreateItem(id, 1);
            }

            _onChange?.Invoke(id);
            
            return value;
        }

        public int Remove(string id, int value=1)
        {
            if (!DefsFacade.I.IsExist(id)) return 0;
            if (value <= 0) return 0;

            var toRemove = value;
            var removed = 0;

            foreach (var item in items.Where(item => item.id == id))
            {
                var valueToRemove = Math.Min(item.value, toRemove);

                item.value -= valueToRemove;
                toRemove -= valueToRemove;
                removed += valueToRemove;

                if (toRemove == 0) break;
            }

            items.RemoveAll(item => item.value == 0);
            
            _onChange?.Invoke(id);

            return removed;
        }

        public int Apply(string id, int value)
        {
            return value >= 0 ? Add(id, value) : Remove(id, -value);
        }

        public int RemoveAll(string id)
        {
            return !DefsFacade.I.IsExist(id) ? 0 : items.RemoveAll(item => item.id == id);
        }

        public bool Has(string id, int atLeast=1)
        {
            return Count(id) >= atLeast;
        }

        public int Count(string id)
        {
            return items
                .FindAll(item => item.id == id)
                .Sum(item => item.value);
        }

        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            if (tags.Length == 0) return items.ToArray();

            return items
                .FindAll(item => tags.All(tag => DefsFacade.I.Items.Get(item.id).HasTag(tag)))
                .ToArray();
        }

        private InventoryItemData CreateItem(string id, int value=0)
        {
            var newItem = new InventoryItemData(id, value);
            items.Add(newItem);

            return newItem;
        }

        private InventoryItemData GetOrCreateItem(string id)
        {
            var found = GetItem(id);
            return found ?? CreateItem(id);
        }

        private InventoryItemData GetItem(string id)
        {
            return items.Find(item => item.id == id);
        }

        public ActionDisposable Subscribe(Action<string> call)
        {
            _onChange += call;
            return new ActionDisposable(() => _onChange -= call);
        }

        public ActionDisposable SubscribeAndInvoke(Action<string> call)
        {
            call?.Invoke("");
            return Subscribe(call);
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] 
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