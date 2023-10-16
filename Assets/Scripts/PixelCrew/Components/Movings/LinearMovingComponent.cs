using UnityEngine;

namespace PixelCrew.Components.Movings
{
    public class LinearMovingComponent : AMovingComponent
    {
        protected override Vector2 CalcNewPosition()
        {
            var position = GetCurrent();
            var direction = GetDirection();
            var speed = GetSpeed();

            position.x += direction * speed;
            
            return position;
        }
    }
}