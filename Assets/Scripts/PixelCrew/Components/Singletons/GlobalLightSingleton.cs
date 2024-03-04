using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Components.Singletons
{
    [RequireComponent(typeof(Light2D))]
    public class GlobalLightSingleton : SingletonMonoBehaviour
    {
        private const float Min = 0.3f;

        private Light2D _light2D;

        protected override void Awake()
        {
            base.Awake();

            _light2D = GetComponent<Light2D>();
        }

        public bool IsInDark()
        {
            return _light2D.intensity <= Min;
        }

        public float GetIntensity()
        {
            return _light2D.intensity;
        }

        public void SetIntensity(float value)
        {
            _light2D.intensity = value;
        }

        public Color GetColor()
        {
            return _light2D.color;
        }

        public void SetColor(Color value)
        {
            _light2D.color = value;
        }
    }
}