using System.Linq;
using PixelCrew.Components.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Controllers
{
    public class SceneController : MonoBehaviour
    {
        private GameSessionComponent _gameSessionComponent;

        private void Start()
        {
            _gameSessionComponent = GameSessionComponent.GetInstance();
        }
        
        public void Reload()
        {
            if (_gameSessionComponent != null) _gameSessionComponent.Load();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}