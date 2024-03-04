using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class ChangeGlobalLightComponent : MonoBehaviour
    {
        private const float Eps = 0.001f;
        
        [SerializeField] private float intensity;
        
        [ColorUsage(true, true)]
        [SerializeField] private Color color;

        [SerializeField] private float duration;

        private Color _defaultColor;
        private float _defaultIntensity;

        private bool _applied = false;
        
        private GlobalLightSingleton _globalLightSingleton;

        private void Start()
        {
            _globalLightSingleton = SingletonMonoBehaviour.GetInstance<GlobalLightSingleton>();
        }

        [ContextMenu("Apply")]
        public void Apply()
        {
            if (_applied) return;
            
            _defaultColor = _globalLightSingleton.GetColor();
            _defaultIntensity = _globalLightSingleton.GetIntensity();

            _globalLightSingleton.SetColor(color);
            _globalLightSingleton.SetIntensity(intensity);

            _applied = true;
        }

        [ContextMenu("Revert")]
        public void Revert()
        {
            if (!_applied) return;
            
            _globalLightSingleton.SetColor(_defaultColor);
            _globalLightSingleton.SetIntensity(_defaultIntensity);
            _applied = false;
        }

        [ContextMenu("ApplyLerp")]
        public void ApplyLerp()
        {
            if (_applied) return;
            if (duration < Eps)
            {
                Apply();
                return;
            }
            
            _defaultColor = _globalLightSingleton.GetColor();
            _defaultIntensity = _globalLightSingleton.GetIntensity();
            _applied = true;
            
            this.DoFrames(duration, progress =>
            {
                var currentColor = Color.Lerp(_defaultColor, color, progress);
                _globalLightSingleton.SetColor(currentColor);

                var currentIntensity = Mathf.Lerp(_defaultIntensity, intensity, progress);
                _globalLightSingleton.SetIntensity(currentIntensity);
            });
        }

        [ContextMenu("RevertLerp")]
        public void RevertLerp()
        {
            if (!_applied) return;
            if (duration < Eps)
            {
                Revert();
                return;
            }
            
            this.DoFrames(duration, progress =>
            {
                var currentColor = Color.Lerp(color, _defaultColor, progress);
                _globalLightSingleton.SetColor(currentColor);

                var currentIntensity = Mathf.Lerp(intensity, _defaultIntensity, progress);
                _globalLightSingleton.SetIntensity(currentIntensity);
            }, () => _applied = false);
        }
    }
}