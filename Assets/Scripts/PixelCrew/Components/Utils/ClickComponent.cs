using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PixelCrew.Components.Utils
{
    public class ClickComponent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent action;
        public void OnPointerClick(PointerEventData eventData)
        {
            action?.Invoke();
        }
    }
}