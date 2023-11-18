using System;
using PixelCrew.Model.Data.Properties.Observable;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    public class QuickInventoryModel
    {
        private readonly PlayerData _data;
        
        public InventoryItemData[] Inventory { get; private set; }
        public readonly IntObservableProperty SelectedIndex = new IntObservableProperty();

        private Action _onChanged = () => { };

        public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = _data.inventory.GetAll();
            _data.inventory.Subscribe(OnInventoryChanged);
        }

        private void OnInventoryChanged(string id)
        {
            // Debug.Log($"INV CHANGED: {id}, {Inventory.Length}");
            // var foundIndex = Array.FindIndex(Inventory, item => item.id == id);
            // Debug.Log(foundIndex);
            // if (foundIndex == -1) return;

            Inventory = _data.inventory.GetAll();
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            _onChanged?.Invoke();
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
    }
}