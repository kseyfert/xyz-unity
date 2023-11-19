using System.Collections.Generic;
using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private QuickInventoryItemWidget prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSessionSingleton _gameSession;
        private readonly List<QuickInventoryItemWidget> _createdItems = new List<QuickInventoryItemWidget>();

        private void Start()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            _trash.Retain(_gameSession.QuickInventoryModel.SubscribeAndInvoke(Rebuild));
        }

        [ContextMenu("Rebuild")]
        private void Rebuild()
        {
            var inventory = _gameSession.QuickInventoryModel.Inventory;
            
            for (var i = _createdItems.Count; i < inventory.Length; i++)
            {
                var item = Instantiate(prefab, container);
                _createdItems.Add(item);
            }

            for (var i = 0; i < inventory.Length; i++)
            {
                _createdItems[i].SetData(inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (var i = inventory.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}