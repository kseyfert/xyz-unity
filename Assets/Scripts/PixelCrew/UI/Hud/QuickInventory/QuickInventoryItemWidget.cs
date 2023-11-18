using PixelCrew.Components.Singletons;
using PixelCrew.Creatures.Model.Definitions;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject selection;
        [SerializeField] private Text value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;
        
        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.id);
            icon.sprite = def.Icon;
            value.text = def.Stackable ? $"x{item.value}" : string.Empty;
            
            var session = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            var quickInventoryModel = session.QuickInventoryModel;
            _trash.Retain(quickInventoryModel.SelectedIndex.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int oldValue, int newValue)
        {
            selection.SetActive(newValue == _index);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}