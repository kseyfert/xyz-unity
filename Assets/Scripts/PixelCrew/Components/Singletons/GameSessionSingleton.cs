using PixelCrew.Creatures.Model;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.Singletons
{
    public class GameSessionSingleton : SingletonMonoBehaviour
    {
        [SerializeField] private PlayerData model;
        public PlayerData Model => model;

        private string _lastSave;

        private void Awake()
        {
            Load<GameSessionSingleton>();
            
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);

            if (GetInstance<GameSessionSingleton>() != this) return;
            
            Save();
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
    }
}