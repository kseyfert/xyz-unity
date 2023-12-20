using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Effects
{
    [RequireComponent(typeof(Collider2D))]
    public class ParallaxEffectManager : MonoBehaviour
    {
        [SerializeField] private string targetTag;

        private Collider2D _collider;
        
        private readonly List<ParallaxEffect> _components = new List<ParallaxEffect>();
        private readonly List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            
            _components.AddRange(GetComponentsInChildren<ParallaxEffect>());
            _sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
        }

        private void LateUpdate()
        {
            foreach (var sprite in _sprites)
            {
                var pos = new Vector3(sprite.transform.position.x, sprite.transform.position.y,
                    _collider.transform.position.z);
                var isInside = _collider.bounds.Contains(pos);
                sprite.color = new Color(1f, 1f, 1f, isInside ? 1f : 0f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(targetTag)) return;
            if (!other.gameObject.CompareTag(targetTag)) return;

            EnableAllComponents();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(targetTag)) return;
            if (!other.gameObject.CompareTag(targetTag)) return;

            DisableAllComponents();
        }
        
        private void EnableAllComponents()
        {
            _components.ForEach(item => item.enabled = true);
        }

        private void DisableAllComponents()
        {
            _components.ForEach(item => item.enabled = false);
        }
    }
}