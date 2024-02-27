using PixelCrew.Components.Singletons;
using PixelCrew.Utils;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        protected override void Init()
        {
            base.Init();

            var cameraSingleton = SingletonMonoBehaviour.GetInstance<CameraSingleton>();
            cameraSingleton.SetFollow(Transform);
            
            if (HealthController == null) return;
            HealthController.onDamage += () => cameraSingleton.Shake();
        }
    }
}