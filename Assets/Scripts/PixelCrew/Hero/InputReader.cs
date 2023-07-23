using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Hero
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private MovementController controller;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            controller.SetDirection(direction);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started) controller.SetJump(true);
            if (context.canceled) controller.SetJump(false);
        }
    }
}