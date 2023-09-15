using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private HeroData heroData;
        
        private string _lastSave = null;
        
        public HeroData Data => heroData;

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

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (GameSession gameSession in sessions)
            {
                if (gameSession != this) return gameSession;
            }

            return null;
        }

        public void Save()
        {
            _lastSave = ToJson();
        }

        public void Load()
        {
            if (_lastSave != null) FromJson(_lastSave);
        }

        private string ToJson()
        {
            return JsonUtility.ToJson(heroData);
        }

        private void FromJson(string json)
        {
            heroData = JsonUtility.FromJson<HeroData>(json);
        }
    }
}