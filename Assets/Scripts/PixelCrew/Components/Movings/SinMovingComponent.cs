using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Components.Movings
{
    public class SinMovingComponent : AMovingComponent
    {
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private bool randomized = false;

        private float _seed;
        
        protected override void Start()
        {
            base.Start();
            if (randomized) _seed = Random.value * Mathf.PI * 2f;
        }

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
                y = original.y + amplitude * Mathf.Sin(_seed + frequency * time)
            };

            return newPosition;
        }
    }
}