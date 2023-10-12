using PixelCrew.Creatures;
using PixelCrew.Creatures.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Controllers
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Creature creature;

        private MovementController _movementController;
        private InteractionController _interactionController;
        private AttackController _attackController;

        private void Start()
        {
            _movementController = creature.MovementController;
            _interactionController = creature.InteractionController;
            _attackController = creature.AttackController;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (_movementController == null) return;
            
            var direction = context.ReadValue<Vector2>();
            _movementController.SetDirection(direction);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (_movementController == null) return;
            
            if (context.started) _movementController.SetJump(true);
            if (context.canceled) _movementController.SetJump(false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (_interactionController == null) return;
            
            if (context.canceled) _interactionController.Interact();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (_attackController == null) return;
            
            if (context.canceled) _attackController.Attack();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (_attackController == null) return;
            
            if (context.canceled) _attackController.Throw();
        }
    }
}