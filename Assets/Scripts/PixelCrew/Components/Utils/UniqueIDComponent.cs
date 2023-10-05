using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class UniqueIDComponent : MonoBehaviour
    {
        [SerializeField] private string id;

        private void Awake()
        {
            if (id == "") id = $"{name}-{GetInstanceID()}";
        }

        public string GetID()
        {
            return id;
        }

    }
}