using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        
        public void DoDestroy()
        {
            Destroy(target);
        }
    }
}