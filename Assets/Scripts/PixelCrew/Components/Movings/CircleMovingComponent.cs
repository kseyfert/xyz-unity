using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.Movings
{
    public class CircleMovingComponent : AMovingComponent
    {
        [SerializeField] private Vector2 center;

        private float _radius;
        private float _startAngle;

        protected override void Start()
        {
            base.Start();
            
            _radius = ((Vector2)transform.position - center).magnitude;

            var angle = Vector2.Angle(GetCurrent() - center, Vector2.right);
            if (center.y >= GetCurrent().y) angle = -angle;
            _startAngle = angle * Mathf.PI / 180;
        }

        protected override Vector2 CalcNewPosition()
        {
            return new Vector2()
            {
                x = center.x + _radius * Mathf.Cos(_startAngle + GetTime()),
                y = center.y + _radius * Mathf.Sin(_startAngle + GetTime())
            };
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = new Color(0, 0, 1, 0.3f);
            Handles.DrawSolidDisc(center, Vector3.forward, 0.1f);
        }
    }
}