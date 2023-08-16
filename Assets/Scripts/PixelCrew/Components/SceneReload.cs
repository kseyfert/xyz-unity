using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class SceneReload : MonoBehaviour
    {
        public void Reload()
        {
            Debug.Log("DDDD");
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}