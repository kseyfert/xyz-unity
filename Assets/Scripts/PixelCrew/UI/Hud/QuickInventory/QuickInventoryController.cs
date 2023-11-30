using System;
using System.Collections.Generic;
using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Image containerImage;
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

            var containerRectTransform = containerImage.GetComponent<RectTransform>();
            var currentSize = containerRectTransform.sizeDelta;
            
            var enabledItems = _createdItems.FindAll(item => item.gameObject.activeSelf);
            var count = Mathf.Max(enabledItems.Count, 3);
            
            // paddingLeft + paddingRight + spacing * count + width * count
            var width = 4 + 2 + 2 * count + 13 * count;
            containerRectTransform.sizeDelta = new Vector2(width, currentSize.y);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}