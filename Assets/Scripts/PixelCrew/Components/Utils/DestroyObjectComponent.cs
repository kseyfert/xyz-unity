using UnityEngine;

namespace PixelCrew.Components.Utils
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        
        public void DoDestroy()
        {
            Destroy(target);
        }
    }
}