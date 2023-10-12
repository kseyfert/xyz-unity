using System.Collections.Generic;
using System.Linq;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class OutOfLevelController : MonoBehaviour
    {
        [SerializeField] private List<string> sceneReloadTags = new List<string>();

        private Scene _scene;

        private void Awake()
        {
            _scene = SceneManager.GetActiveScene();
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            Process(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Process(other.gameObject);
        }

        private void Process(GameObject obj)
        {
            if (sceneReloadTags != null)
            {
                if (obj.CompareTags(sceneReloadTags))
                {
                    SceneManager.LoadScene(_scene.name);
                    return;
                }
            }
            Destroy(obj);
        }
    }
}