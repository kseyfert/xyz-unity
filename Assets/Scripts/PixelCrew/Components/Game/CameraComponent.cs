using System;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class CameraComponent : MonoBehaviour
    {
        private static CameraComponent _instance;
        public static CameraComponent I => _instance == null ? LoadInstance() : _instance;

        private delegate void CameraDelegate();
        private static event CameraDelegate OnLoaded;

        [SerializeField] private bool main;

        private static CameraComponent LoadInstance()
        {
            var cameras = FindObjectsOfType<CameraComponent>();
            
            var mainIndex = Array.FindIndex(cameras, item => item.main);
            mainIndex = mainIndex == -1 ? 0 : mainIndex;

            _instance = cameras[mainIndex];
            OnLoaded?.Invoke();
            return _instance;
        }

        private void Awake()
        {
            OnLoaded += OnInstanceLoaded;
            LoadInstance();
        }

        private void OnInstanceLoaded()
        {
            if (this != _instance) gameObject.SetActive(false);

            OnLoaded -= OnInstanceLoaded;
        }

        private void OnDestroy()
        {
            OnLoaded -= OnInstanceLoaded;
        }
    }
}