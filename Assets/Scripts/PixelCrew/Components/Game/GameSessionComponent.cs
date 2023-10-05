using System;
using System.Collections.Generic;
using PixelCrew.Creatures.Model;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class GameSessionComponent : MonoBehaviour
    {
        [Serializable]
        public struct ModelItem
        {
            public string id;
            public CreatureModel model;
        }
        [SerializeField] private List<ModelItem> creatures = new List<ModelItem>();

        private readonly List<string> _lastSave = new List<string>();
    
        private void Awake()
        {
            var gs = GetExistsSession();
            if (gs != null)
            {
                gs.Save();
                Destroy(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(gameObject);
            }
        }

        private GameSessionComponent GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSessionComponent>();
            foreach (GameSessionComponent gameSession in sessions)
            {
                if (gameSession != this) return gameSession;
            }

            return null;
        }

        public CreatureModel GetCreatureModel(string id)
        {
            var index = creatures.FindIndex(item => item.id == id);
            if (index >= 0) return creatures[index].model;
            
            ModelItem newItem;
            newItem.id = id;
            newItem.model = new CreatureModel();
            creatures.Add(newItem);

            return newItem.model;
        }

        public void Save()
        {
            _lastSave.RemoveAll(item => true);
            foreach (var item in creatures)
            {
                _lastSave.Add(JsonUtility.ToJson(item));
            }
        }

        public void Load()
        {
            if (_lastSave.Count == 0) return;
            
            creatures.RemoveAll(item => true);
            foreach (var item in _lastSave)
            {
                var modelItem = JsonUtility.FromJson<ModelItem>(item);
                creatures.Add(modelItem);
            }
        }
    }
}