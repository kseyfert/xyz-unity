using System.Collections.Generic;
using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private QuickInventoryItemWidget prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSessionComponent _gameSession;
        private InventoryItemData[] _inventory;
        private List<QuickInventoryItemWidget> _createdItems = new List<QuickInventoryItemWidget>();

        private void Start()
        {
            _gameSession = GameSessionComponent.GetInstance();
            Rebuild();
        }

        [ContextMenu("Rebuild")]
        private void Rebuild()
        {
            _inventory = _gameSession.GetCreatureModel("captain").inventory.GetAll();
            
            for (var i = _createdItems.Count; i < _inventory.Length; i++)
            {
                var item = Instantiate(prefab, container);
                _createdItems.Add(item);
            }

            for (var i = 0; i < _inventory.Length; i++)
            {
                _createdItems[i].SetData(_inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (var i = _inventory.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }
    }
}