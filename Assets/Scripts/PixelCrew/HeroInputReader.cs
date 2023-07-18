using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {
        private Hero _hero;

        private void Awake()
        {
            _hero = GetComponent<Hero>();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started || context.performed) _hero.SetJump(true);
            if (context.canceled) _hero.SetJump(false);
        }
    }
}