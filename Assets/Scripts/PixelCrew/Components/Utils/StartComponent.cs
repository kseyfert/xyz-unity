using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils
{
    public class StartComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent action;

        private void Start()
        {
            action?.Invoke();
        }
    }
}