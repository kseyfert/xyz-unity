using PixelCrew.Components.Singletons;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private CustomButton buyButton;
        [SerializeField] private CustomButton useButton;
        [SerializeField] private ItemWidget price;

        [SerializeField] private Text info;
        [SerializeField] private Transform perksContainer;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSessionSingleton _gameSession;
        private string _lastSelectedPerkId;
        
        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;

            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(perksContainer);
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            
            _dataGroup.SetData(DefsFacade.I.Perks.All);
            foreach (var item in _dataGroup.GetActiveItems())
            {
                var disposable = item.Subscribe(() => OnPerkSelected(item.Id));
                _trash.Retain(disposable);
            }
            _dataGroup.GetActiveItems()[0].Select();
        }

        private void OnPerkSelected(string id)
        {
            _lastSelectedPerkId = id;
            UpdateView();
        }

        private void UpdateView()
        {
            var id = _lastSelectedPerkId;
            
            foreach (var item in _dataGroup.GetActiveItems())
            {
                if (item.Id != id) item.Deselect();
                item.UpdateView();
            }
            
            var def = DefsFacade.I.Perks.Get(id);
            info.text = def.Info;
            price.Set(def.Price);

            buyButton.interactable = true;
            if (_gameSession.PerksModel.IsUnlocked(id)) buyButton.interactable = false;
            if (!_gameSession.PerksModel.CanUnlock(id)) buyButton.interactable = false;
   
            useButton.interactable = true;
            if (_gameSession.PerksModel.IsUsed(id)) useButton.interactable = false;
            if (_gameSession.PerksModel.IsLocked(id)) useButton.interactable = false;
        }

        public void OnUse()
        {
            _gameSession.PerksModel.Use(_lastSelectedPerkId);
            UpdateView();
        }

        public void OnBuy()
        {
            _gameSession.PerksModel.Unlock(_lastSelectedPerkId);
            UpdateView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Time.timeScale = _defaultTimeScale;
            _trash.Dispose();
        }
    }
}