using PixelCrew.Components.Game;
using PixelCrew.Creatures.Model.Definitions;
using PixelCrew.UI.PauseMenu;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget healthBar;
        
        private PauseMenuWindow _activePauseMenu;
        private GameSessionComponent _gameSessionComponent;

        private void Start()
        {
            _gameSessionComponent = GameSessionComponent.GetInstance();
            _gameSessionComponent.GetCreatureModel("captain").hp.OnChanged += OnHpChanged;
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
    }
}