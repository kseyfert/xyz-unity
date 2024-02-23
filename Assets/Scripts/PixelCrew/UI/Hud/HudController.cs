using PixelCrew.Components.Singletons;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.PauseMenu;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Hud
{
    [RequireComponent(typeof(Canvas))]
    public class HudController : MonoBehaviour
    {
        private const string Coins = "Coins";
        
        [SerializeField] private ProgressBarWidget healthBar;
        [SerializeField] private ItemWidget balance;
        
        private PauseMenuWindow _activePauseMenu;
        private GameSessionSingleton _gameSession;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            _trash.Retain(_gameSession.Model.hp.SubscribeAndInvoke(OnHpChanged));
            _trash.Retain(_gameSession.Model.inventory.SubscribeAndInvoke(OnInventoryChanged));
        }

        private void OnHpChanged(int oldValue, int newValue)
        {
            var hpLevels = DefsFacade.I.Player.GetStat(StatId.Hp).Levels;
            var maxHealth = hpLevels[hpLevels.Length - 1].Value;
            
            var value = (float)newValue / maxHealth;
            healthBar.SetProgress(value);
        }

        private void OnInventoryChanged(string id)
        {
            var value = _gameSession.Model.inventory.Count(Coins);
            balance.Set(Coins, value);
        }

        public void OnPauseRequested(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (_activePauseMenu == null)
            {
                _activePauseMenu = (PauseMenuWindow) AnimatedWindow.Open("UI/PauseMenuWindow");
                return;
            }

            _activePauseMenu.onClosed += (window) => _activePauseMenu = null;
            AnimatedWindow.CloseAll();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}