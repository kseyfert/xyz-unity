using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReplaceableParticleSystemComponent : MonoBehaviour
    {
        [SerializeField] private float collisionTimeout;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;

        private ParticleSystem _particleSystem;
        private List<ParticleCollisionEvent> _collisions;
        private float _collisionTimer = 0;
        
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
                var newObject = Instantiate(prefab, _collisions[i].intersection, Quaternion.identity);
                newObject.transform.SetParent(parent);
            }
        }
        
        private void Update()
        {
            if (_collisionTimer >= Time.time) return;

            var collisionModule = _particleSystem.collision;
            collisionModule.enabled = true;
        }

        public void RunBurst(int count)
        {
            var burst = _particleSystem.emission.GetBurst(0);
            burst.count = count;
            _particleSystem.emission.SetBurst(0, burst);

            var collisionModule = _particleSystem.collision;
            collisionModule.enabled = false;
            _collisionTimer = Time.time + collisionTimeout;
            
            _particleSystem.Play();
        }
    }
}