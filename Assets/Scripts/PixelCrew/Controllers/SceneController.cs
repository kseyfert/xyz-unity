using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Controllers
{
    public class SceneController : MonoBehaviour
    {
        private GameSessionSingleton _gameSession;

        private void Start()
        {
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
        }
        
        public void Reload()
        {
            if (_gameSession != null) _gameSession.Load();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}