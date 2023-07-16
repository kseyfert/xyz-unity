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

        private void OnMovement(InputValue value)
        {
            var direction = value.Get<Vector2>();
            _hero.SetDirection(direction);
        }
    }
}