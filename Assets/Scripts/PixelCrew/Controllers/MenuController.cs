using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.Controllers
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private EventSystem eventSystem;

        private bool _opening = false;

        public void OnOpen()
        {
            if (_opening) return;

            _opening = true;
            
            canvas.gameObject.SetActive(true);
            eventSystem.gameObject.SetActive(true);

            var window = Resources.Load("UI/MainMenuWindow");
            Instantiate(window, canvas.transform);
        }        
    }
}