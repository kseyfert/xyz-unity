using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Hero
{
    [RequireComponent(typeof(CoinsController))]
    public class HitCoinsParticle : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private int value = 5;
        [SerializeField] private float collisionTimeout = 0;

        private ParticleSystem _particleSystem;
        private List<ParticleCollisionEvent> _collisions;
        private CoinsController _coinsController;

        private float _collisionTimer = 0;

        private void Awake()
        {
            _coinsController = GetComponent<CoinsController>();
            _particleSystem = GetComponent<ParticleSystem>();
            _collisions = new List<ParticleCollisionEvent>();
        }

        public void Spawn()
        {
            var amount = _coinsController.GetAmount();
            var amountToSpawn = Math.Min(value, amount);
            if (amountToSpawn <= 0) return;
            
            _coinsController.SubAmount(amountToSpawn);

            var burst = _particleSystem.emission.GetBurst(0);
            burst.count = amountToSpawn;
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