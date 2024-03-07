using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Utils.Checks
{
    public class CircleOverlapCheckComponent : MonoBehaviour
    {
        [SerializeField] private float radius = 1f;
        [SerializeField] private LayerMask layers;

        private readonly Collider2D[] _overlapResult = new Collider2D[100];
        
        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _overlapResult, layers);
            var overlaps = new List<GameObject>();
            for (var i = 0; i < size; i++)
            {
                overlaps.Add(_overlapResult[i].gameObject);
            }

            return overlaps.ToArray();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);
        }
#endif

    }
}