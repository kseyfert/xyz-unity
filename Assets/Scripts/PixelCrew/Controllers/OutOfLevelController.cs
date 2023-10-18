using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SceneController))]
    public class OutOfLevelController : MonoBehaviour
    {
        [SerializeField] private List<string> sceneReloadTags = new List<string>();

        private SceneController _sceneController;

        private void Awake()
        {
            _sceneController = GetComponent<SceneController>();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var go = other.gameObject;
            if (!go.CompareTags(sceneReloadTags)) return;

            var goCollider = go.GetComponent<Collider2D>();
            if (goCollider == null) return;
            if (!goCollider.enabled) return;
            
            _sceneController.Reload();
            Destroy(go);
        }
    }
}