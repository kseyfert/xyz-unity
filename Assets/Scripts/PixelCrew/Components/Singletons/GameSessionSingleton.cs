using System;
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
        public PlayerData Model => model;
        
        public QuickInventoryModel QuickInventoryModel { get; private set; }

        private CompositeDisposable _trash = new CompositeDisposable();
        private string _lastSave;

        private static bool hudLoaded = false;
        
        private void Awake()
        {
            Load<GameSessionSingleton>();

            if (SceneManager.GetActiveScene().name != "Hud" && !hudLoaded)
            {
                SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
                hudLoaded = true;
            }

            if (GetInstance<GameSessionSingleton>() != this) return;
            
            Save();
            QuickInventoryModel = new QuickInventoryModel(model);
            _trash.Retain(QuickInventoryModel);
            
            DontDestroyOnLoad(gameObject);
        }

        public void Save()
        {
            _lastSave = JsonUtility.ToJson(model);
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(_lastSave)) return;

            model = JsonUtility.FromJson<PlayerData>(_lastSave);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}