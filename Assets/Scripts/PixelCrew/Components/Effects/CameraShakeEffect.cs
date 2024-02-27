using System.Collections;
using Cinemachine;
using UnityEngine;

namespace PixelCrew.Components.Effects
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShakeEffect : MonoBehaviour
    {
        [SerializeField] private float time = 0.3f;
        [SerializeField] private float intensity = 3f;

        private CinemachineBasicMultiChannelPerlin _cameraNoise;
        private Coroutine _coroutine;

        private void Awake()
        {
            var virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _cameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            if (_coroutine != null) StopAnimation();
            _coroutine = StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            _cameraNoise.m_FrequencyGain = intensity;
            yield return new WaitForSeconds(time);
            StopAnimation();
        }

        private void StopAnimation()
        {
            _cameraNoise.m_FrequencyGain = 0;
            StopCoroutine(_coroutine);
        }
    }
}