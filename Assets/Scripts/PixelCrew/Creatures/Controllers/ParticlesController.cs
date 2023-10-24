using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class ParticlesController : AController
    {
        [Serializable]
        private struct SpawnItem
        {
            public string name;
            public bool disabled;
            public SpawnComponent component;
        }
        
        [SerializeField] private List<SpawnItem> spawns;

        public void Spawn(string particleName)
        {
            var index = spawns.FindIndex(item => item.name == particleName);
            if (index < 0) return;

            SpawnItem spawnItem = spawns[index];
            if (spawnItem.disabled) return;
            
            spawnItem.component.Spawn();
        }

        public void SpawnSeq(string particleName, int count, float timeout)
        {
            StartCoroutine(SpawnSeqCoroutine(particleName, count, timeout));
        }

        private IEnumerator SpawnSeqCoroutine(string particleName, int count, float timeout)
        {
            for (var i = 0; i < count; i++)
            {
                Spawn(particleName);
                yield return new WaitForSeconds(timeout);
            }
        }

        public override void Die()
        {
        }
    }
}