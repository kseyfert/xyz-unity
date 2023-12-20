using Cinemachine;
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
    }
}