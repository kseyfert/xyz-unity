using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private List<LevelProgress> progress;
        
        public int GetLevel(StatId id)
        {
            if (progress == null) return 0;
            
            var found = progress.FirstOrDefault(item => item.Id == id);
            return found?.Level ?? 0;
        }

        public void LevelUp(StatId id)
        {
            if (progress == null) progress = new List<LevelProgress>();
            
            var found = progress.FirstOrDefault(item => item.Id == id);
            if (found == null)
            {
                found = new LevelProgress(id);
                progress.Add(found);
            }
            
            found.IncLevel();
        }
    }

    [Serializable]
    public class LevelProgress
    {
        [SerializeField] private StatId id;
        [SerializeField] private int level;

        public StatId Id => id;
        public int Level => level;

        public LevelProgress(StatId id)
        {
            this.id = id;
            level = 0;
        }

        public void IncLevel()
        {
            level++;
        }
    }
}