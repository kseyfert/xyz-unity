using PixelCrew.UI.PauseMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        private PauseMenuWindow _activePauseMenu;

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