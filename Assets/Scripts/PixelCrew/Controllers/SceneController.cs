using PixelCrew.Components.Singletons;
using PixelCrew.UI.LevelLoader;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Controllers
{
    public class SceneController : MonoBehaviour
    {
        private LevelLoaderSingleton _levelLoader;

        private void Start()
        {
            _levelLoader = SingletonMonoBehaviour.GetInstance<LevelLoaderSingleton>();
        }
        
        public void Reload()
        {
            var scene = SceneManager.GetActiveScene();
            _levelLoader.Show(scene.name);
        }

        public void LoadScene(string sceneName)
        {
            _levelLoader.Show(sceneName);
        }
    }
}