using PixelCrew.Components.Singletons;
using PixelCrew.Creatures.Model.Definitions;
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
        [SerializeField] private ProgressBarWidget healthBar;
        
        private PauseMenuWindow _activePauseMenu;
        private GameSessionSingleton _gameSession;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            _trash.Retain(_gameSession.Model.hp.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged(int oldValue, int newValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            healthBar.SetProgress(value);
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