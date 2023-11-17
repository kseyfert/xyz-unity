using UnityEngine;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicComponent : MonoBehaviour
    {
        private static MusicComponent _instance;
        public static MusicComponent I => _instance == null ? LoadInstance() : _instance;

        private delegate void MusicDelegate();
        private static event MusicDelegate OnLoaded;

        private static MusicComponent LoadInstance()
        {
            _instance = FindObjectOfType<MusicComponent>();
            OnLoaded?.Invoke();
            return _instance;
        }

        private AudioSource _source;
        public AudioSource Source => _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
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