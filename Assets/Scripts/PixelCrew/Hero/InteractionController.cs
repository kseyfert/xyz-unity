using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Hero
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private CheckCircleOverlap interactionPosition;
        
        public void Interact()
        {
            if (interactionPosition == null) return;

            var gos = interactionPosition.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var interactable = item.GetComponent<InteractableComponent>();
                if (interactable == null) continue;
                
                interactable.Interact();
            }
        }
    }
}