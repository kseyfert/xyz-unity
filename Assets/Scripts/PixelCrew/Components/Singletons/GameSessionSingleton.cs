using System;
using System.Collections.Generic;
using PixelCrew.Components.Game;
using PixelCrew.Creatures;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.Singletons
{
    public class GameSessionSingleton : SingletonMonoBehaviour
    {
        [SerializeField] private PlayerData model;
        [SerializeField] private List<string> checkpoints = new List<string>();
        
        public PlayerData Model => model;
        public QuickInventoryModel QuickInventoryModel { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        [SerializeField] private string _lastSave;

        protected override void Awake()
        {
            base.Awake();
            
            PixelCrew.Utils.Utils.AddScene("Hud");
            
            var mainGameSession = GetInstance<GameSessionSingleton>();
            mainGameSession.SpawnHero();
            if (mainGameSession != this) return;
            
            QuickInventoryModel = new QuickInventoryModel(model);
            _trash.Retain(QuickInventoryModel);

            PerksModel = new PerksModel(model);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(model);
            _trash.Retain(StatsModel);
            
            DontDestroyOnLoad(gameObject);
        }

        public void Save()
        {
            _lastSave = JsonUtility.ToJson(model);
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(_lastSave)) return;
            
            model.UpdateFromJson(_lastSave);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _trash.Dispose();
        }

        public void SetCheckpoint(string checkpointID, bool value)
        {
            var scene = SceneManager.GetActiveScene().name;
            var id = $"{scene}/{checkpointID}";

            if (!value)
            {
                checkpoints.Remove(id);
                return;
            }

            Save();
            if (!checkpoints.Contains(id)) checkpoints.Add(id);
        }

        public bool IsCheckpointChecked(string checkpointID)
        {
            var scene = SceneManager.GetActiveScene().name;
            var id = $"{scene}/{checkpointID}";
            
            return checkpoints.Contains(id);
        }

        private void SpawnHero()
        {
            var scene = SceneManager.GetActiveScene().name;
            
            var existHero = FindObjectOfType<Hero>();
            if (existHero != null) return;
            
            var cps = FindObjectsOfType<CheckpointComponent>();
            if (cps.Length == 0) return;
            
            CheckpointComponent chosenCheckpoint = null;

            if (checkpoints.Count > 0)
            {
                var lastIndex = checkpoints.FindLastIndex(item => item.StartsWith(scene));
                if (lastIndex >= 0)
                {
                    var checkpointId = checkpoints[lastIndex].Split('/')[1];
                    var cpIndex = Array.FindIndex(cps, item => item.Id == checkpointId);
                    if (cpIndex >= 0) chosenCheckpoint = cps[cpIndex];
                }
            }

            if (chosenCheckpoint == null)
            {
                var defaultIndex = Array.FindIndex(cps, item => item.IsDefault);
                if (defaultIndex >= 0) chosenCheckpoint = cps[defaultIndex];
            }

            if (chosenCheckpoint == null)
            {
                chosenCheckpoint = cps[0];
            }

            Load();
            chosenCheckpoint.SpawnHero();
        }
    }
}