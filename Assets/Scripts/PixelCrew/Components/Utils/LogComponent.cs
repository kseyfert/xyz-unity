using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class LogComponent : MonoBehaviour
    {
        [SerializeField] private string message;

        [ContextMenu("Log")]
        public void Log(GameObject go=null)
        {
            Debug.Log($"{go} - {message}");
        }
    }
}