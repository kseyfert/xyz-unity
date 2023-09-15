using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class SceneReload : MonoBehaviour
    {
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }
        
        public void Reload()
        {
            _gameSession.Load();
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}