using System;
using PixelCrew.Components.Singletons;
using PixelCrew.Components.Utils;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.PlayerStats
{
    [RequireComponent(typeof(ClickComponent))]
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text statName;
        [SerializeField] private Text currentValue;
        [SerializeField] private Text increaseValue;
        [SerializeField] private ProgressBarWidget progressFilling;
        [SerializeField] private GameObject select;

        private GameSessionSingleton _gameSession;
        private StatDef _data;
        public string Id => _data.Id.ToString();
        
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
        
        public void SetData(StatDef data, int index)
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
            statName.text = $"{_data.Name}:";

            var curVal = _gameSession.StatsModel.GetCurrentLevelDef(_data.Id).Value;
            currentValue.text = $"{curVal}";

            increaseValue.gameObject.SetActive(false);
            
            var nextVal = _gameSession.StatsModel.GetNextLevelDef(_data.Id).Value;
            var inc = nextVal - curVal;
            increaseValue.text = $"+{inc}";
            if (_gameSession.StatsModel.HasNextLevel(_data.Id)) increaseValue.gameObject.SetActive(true);

            var lastDef = _gameSession.StatsModel.GetLastLevelDef(_data.Id);
            var progress = curVal / lastDef.Value;
            
            progressFilling.SetProgress(progress);
            select.SetActive(_isSelected);
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