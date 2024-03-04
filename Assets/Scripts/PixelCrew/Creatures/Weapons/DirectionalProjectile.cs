using PixelCrew.Components.Movings;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class DirectionalProjectile : AMovingComponent
    {
        [SerializeField] private Vector2 direction = Vector2.left;
        
        public void SetDirection(Vector2 dir)
        {
            direction = dir;
        }

        protected override Vector2 CalcNewPosition()
        {
            var position = GetCurrent();
            var speed = GetSpeed();

            position += direction * speed;

            return position;
        }
    }
}