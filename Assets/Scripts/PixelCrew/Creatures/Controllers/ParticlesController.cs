using System;
using System.Collections.Generic;
using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class ParticlesController : AController
    {
        [SerializeField] private Creature creature;
        
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

        protected override Creature GetCreature()
        {
            return creature;
        }

        public override void Die()
        {
        }
    }
}