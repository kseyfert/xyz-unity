using System;
using PixelCrew.Components.Singletons;
using PixelCrew.Components.Utils;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    [RequireComponent(typeof(ClickComponent))]
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image locked;
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject used;

        private GameSessionSingleton _gameSession;
        private PerkDef _data;
        public string Id => string.IsNullOrEmpty(_data.Id) ? null : _data.Id;
        
        private Action _onSelected = () => { };
        private bool _isSelected;
        public bool IsSelected => _isSelected;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            var button = GetComponent<ClickComponent>();
            _trash.Retain(button.Subscribe(Select));

            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            UpdateView();
        }

        public void SetData(PerkDef data, int index)
        {
            _data = data;
            UpdateView();
        }

        public void Select()
        {
            _onSelected?.Invoke();
            _isSelected = true;
            UpdateView();
        }

        public void Deselect()
        {
            _isSelected = false;
            UpdateView();
        }

        public void UpdateView()
        {
            if (_gameSession == null) return;
            
            icon.sprite = _data.Icon;
            locked.sprite = _data.Icon;
            locked.gameObject.SetActive(_gameSession.PerksModel.IsLocked(_data.Id));
            selected.SetActive(_isSelected);
            used.SetActive(_gameSession.PerksModel.IsUsed(_data.Id));
        }
        
        public ActionDisposable Subscribe(Action call)
        {
            _onSelected += call;
            return new ActionDisposable(() => _onSelected -= call);
        }

        public ActionDisposable SubscribeAndInvoke(Action call)
        {
            call?.Invoke();
            return Subscribe(call);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}