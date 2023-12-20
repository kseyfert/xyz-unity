using PixelCrew.Components.Game;
using PixelCrew.Components.Utils.Checks;
using PixelCrew.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Creatures.Controllers
{
    public class InteractionController : AController
    {
        [SerializeField] private CircleOverlapCheckComponent interactionChecker;
        
        public void Interact()
        {
            if (interactionChecker == null) return;

            var gos = interactionChecker.GetObjectsInRange();
            foreach (GameObject item in gos)
            {
                var interactable = item.GetComponent<InteractableComponent>();
                if (interactable == null) continue;
                
                interactable.Interact(Creature.gameObject);
            }
        }
    }
}