using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        protected override void Init()
        {
            base.Init();

            var cameraSingleton = SingletonMonoBehaviour.GetInstance<CameraSingleton>();
            cameraSingleton.SetFollow(Transform);

            if (HealthController != null) HealthController.onDamage += () => cameraSingleton.Shake();
            
            var globalLight = SingletonMonoBehaviour.GetInstance<GlobalLightSingleton>();
            var light2D = GetComponentInChildren<Light2D>(true);
            if (light2D != null) light2D.gameObject.SetActive(globalLight.IsInDark());
        }
    }
}