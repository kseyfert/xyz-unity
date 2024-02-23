using PixelCrew.Components.Singletons;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.PlayerStats
{
    public class PlayerStatsWindow : AnimatedWindow
    {
        [SerializeField] private Transform statsContainer;
        [SerializeField] private StatWidget prefab;

        [SerializeField] private CustomButton buyButton;
        [SerializeField] private ItemWidget price;

        private DataGroup<StatDef, StatWidget> _dataGroup;
        private string _lastSelectedId;

        private GameSessionSingleton _gameSession;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;

            _dataGroup = new DataGroup<StatDef, StatWidget>(prefab, statsContainer);
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            
            _dataGroup.SetData(DefsFacade.I.Player.Stats);
            foreach (var item in _dataGroup.GetActiveItems())
            {
                var disposable = item.Subscribe(() => OnStatSelected(item.Id));
                _trash.Retain(disposable);
            }
            _dataGroup.GetActiveItems()[0].Select();
            
            _trash.Retain(_gameSession.StatsModel.Subscribe(OnStatsChanged));
        }

        private void OnStatsChanged()
        {
            UpdateView();
        }

        private void OnStatSelected(string id)
        {
            _lastSelectedId = id;
            UpdateView();
        }

        private void UpdateView()
        {
            var id = _lastSelectedId;
            
            foreach (var item in _dataGroup.GetActiveItems())
            {
                if (item.Id != id) item.Deselect();
                item.UpdateView();
            }

            price.gameObject.SetActive(false);
            if (_gameSession.StatsModel.HasNextLevel(id))
            {
                var nextDef = _gameSession.StatsModel.GetNextLevelDef(id);
                price.Set(nextDef.Price);
                price.gameObject.SetActive(true);
            }
            
            buyButton.interactable = _gameSession.StatsModel.CanLevelUp(id);
        }

        public void OnBuy()
        {
            _gameSession.StatsModel.LevelUp(_lastSelectedId);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Time.timeScale = _defaultTimeScale;
            _trash.Dispose();
        }
    }
}