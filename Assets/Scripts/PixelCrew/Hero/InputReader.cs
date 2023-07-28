using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PixelCrew.Hero
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private MovementController movementController;
        [SerializeField] private InteractionController interactionController;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            movementController.SetDirection(direction);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started) movementController.SetJump(true);
            if (context.canceled) movementController.SetJump(false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled) interactionController.Interact();
        }
    }
}