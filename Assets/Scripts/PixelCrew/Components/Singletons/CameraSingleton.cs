using PixelCrew.Utils;

namespace PixelCrew.Components.Singletons
{
    public class CameraSingleton : SingletonMonoBehaviour
    {
        private void Start()
        {
            Load<CameraSingleton>();
        }
    }
}