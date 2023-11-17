using PixelCrew.Creatures.Model.Data;
using PixelCrew.Creatures.Model.Definitions;
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
            
        }
    }
}