using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ChestComponent : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private int value = 5;
        [SerializeField] private float collisionTimeout = 1.5f;

        private ParticleSystem _particleSystem;
        private List<ParticleCollisionEvent> _collisions;

        private float _collisionTimer = 0;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _collisions = new List<ParticleCollisionEvent>();
        }

        public void Open()
        {
            var burst = _particleSystem.emission.GetBurst(0);
            burst.count = value;
            _particleSystem.emission.SetBurst(0, burst);

            var collisionModule = _particleSystem.collision;
            collisionModule.enabled = false;
            _collisionTimer = Time.time + collisionTimeout;
            
            _particleSystem.Play();
        }

        private void Update()
        {
            if (_collisionTimer >= Time.time) return;

            var collisionModule = _particleSystem.collision;
            collisionModule.enabled = true;
        }

        private void OnParticleCollision(GameObject other)
        {
            int num = _particleSystem.GetCollisionEvents(other, _collisions);
            for (var i = 0; i < num; i++)
            {
                Instantiate(coinPrefab, _collisions[i].intersection, Quaternion.identity);
            }
        }
    }
}