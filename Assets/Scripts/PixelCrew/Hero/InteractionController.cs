using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Hero
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private LayerMask layers;
        [SerializeField] private float radius = 0.3f;
        
        public void Interact()
        {
            var overlapResults = new Collider2D[1];
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, overlapResults, layers);
            
            for (var i = 0; i < size; i++)
            {
                var item = overlapResults[i];
                var interactable = item.GetComponent<InteractableComponent>();
                if (interactable != null) interactable.Interact();
            }
        }
    }
}