using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReplaceableParticleSystem : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private ParticleSystem _particleSystem;
        private List<ParticleCollisionEvent> _collisions;
        
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _collisions = new List<ParticleCollisionEvent>();
        }
        
        private void OnParticleCollision(GameObject other)
        {
            if (_particleSystem == null) return;
            
            int num = _particleSystem.GetCollisionEvents(other, _collisions);
            for (var i = 0; i < num; i++)
            {
                Instantiate(prefab, _collisions[i].intersection, Quaternion.identity);
            }
        }
    }
}