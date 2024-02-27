using Cinemachine;
using PixelCrew.Components.Effects;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Singletons
{
    public class CameraSingleton : SingletonMonoBehaviour
    {
        public void SetFollow(Transform obj)
        {
            var cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();
            cinemachine.Follow = obj;
        }

        public void Shake()
        {
            var shaker = GetComponentInChildren<CameraShakeEffect>();
            if (shaker == null) return;
            
            shaker.Shake();
        }
    }
}