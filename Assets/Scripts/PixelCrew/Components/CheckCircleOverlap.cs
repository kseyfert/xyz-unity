using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float radius = 1f;
        [SerializeField] private LayerMask layers;

        private readonly Collider2D[] _overlapResult = new Collider2D[5];
        
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

        private void OnDrawGizmosSelected()
        {
            Handles.color = new Color(1, 0, 0, 0.3f);
            Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);
        }
    }
}