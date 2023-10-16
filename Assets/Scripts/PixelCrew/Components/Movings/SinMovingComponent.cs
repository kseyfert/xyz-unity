using UnityEngine;

namespace PixelCrew.Components.Movings
{
    public class SinMovingComponent : AMovingComponent
    {
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 1f;
        
        protected override Vector2 CalcNewPosition()
        {
            var original = GetOriginal();
            var position = GetCurrent();
            var speed = GetSpeed();
            var direction = GetDirection();
            var time = GetTime();

            var newPosition = new Vector2
            {
                x = position.x + direction * speed,
                y = original.y + amplitude * Mathf.Sin(frequency * time)
            };

            return newPosition;
        }
    }
}