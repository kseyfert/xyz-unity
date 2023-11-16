using PixelCrew.UI.PauseMenu;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        private PauseMenuWindow _activePauseMenu;
        
        private void Start()
        {
            
        }

        public void OnPauseRequested(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            if (_activePauseMenu == null)
            {
                _activePauseMenu = (PauseMenuWindow) AnimatedWindow.Open("UI/PauseMenuWindow");
                return;
            }

            _activePauseMenu.Close();
            _activePauseMenu.onClosed += (window) => _activePauseMenu = null; 
        }
    }
}