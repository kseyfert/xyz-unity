using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties.Observable;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PerksData
    {
        [SerializeField] private StringObservableProperty used = new StringObservableProperty();
        [SerializeField] private List<string> unlocked = new List<string>();

        public StringObservableProperty Used => used;

        public void Unlock(string id)
        {
            if (!unlocked.Contains(id)) unlocked.Add(id);
        }

        public void Use(string id)
        {
            if (!unlocked.Contains(id)) return;
            if (used.Value == id) return;

            used.Value = id;
        }

        public bool IsUnlocked(string id)
        {
            return unlocked.Contains(id);
        }

        public bool IsLocked(string id)
        {
            return !IsUnlocked(id);
        }
    }
}