using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class OutOfLevelComponent : MonoBehaviour
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
                if (sceneReloadTags.Any(obj.CompareTag))
                {
                    SceneManager.LoadScene(_scene.name);
                    return;
                }
            }
            Destroy(obj);
        }
    }
}