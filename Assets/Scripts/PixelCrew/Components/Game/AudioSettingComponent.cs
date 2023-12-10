using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties.Persistent;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private GameSettingsKeys mode = GameSettingsKeys.Music;

        private AudioSource _source;
        private FloatPersistentProperty _model;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            Init();
        }

        private void Init()
        {
            _model = GetProperty();
            _model.OnChanged += OnModelValueChanged;
            OnModelValueChanged(_model.Value, _model.Value);
        }

        private void OnModelValueChanged(float oldValue, float newValue)
        {
            if (_source == null) return;
            
            _source.volume = newValue;
        }

        private FloatPersistentProperty GetProperty()
        {
            switch (mode)
            {
                case GameSettingsKeys.Music: return GameSettings.I.MusicVolume;
                case GameSettingsKeys.Sfx: return GameSettings.I.SfxVolume;
                default: return null;
            }
        }

        private void OnValidate()
        {
            if (_model == null) return;
            
            var m = GetProperty();
            if (m == _model) return;

            _model.OnChanged -= OnModelValueChanged;
            Init();
        }

        private void OnDestroy()
        {
            if (_model == null) return;
            _model.OnChanged -= OnModelValueChanged;
        }
    }
}