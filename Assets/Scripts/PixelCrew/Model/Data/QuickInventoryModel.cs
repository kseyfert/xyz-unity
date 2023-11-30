using System;
using PixelCrew.Model.Data.Properties.Observable;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        
        public InventoryItemData[] Inventory { get; private set; }
        public readonly IntObservableProperty SelectedIndex = new IntObservableProperty();

        private Action _onChanged = () => { };

        public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];
        public ItemDef SelectedItemDef => DefsFacade.I.Items.Get(SelectedItem.id);

        private CompositeDisposable _trash = new CompositeDisposable();

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = _data.inventory.GetAll(ItemTag.Usable);
            _trash.Retain(_data.inventory.Subscribe(OnInventoryChanged));
        }

        private void OnInventoryChanged(string id)
        {
            var def = DefsFacade.I.Items.Get(id);
            if (!def.HasTag(ItemTag.Usable)) return;
            
            Inventory = _data.inventory.GetAll(ItemTag.Usable);
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

        public void SetNextItem()
        {
            SelectedIndex.Value = (SelectedIndex.Value + 1) % Inventory.Length;
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}