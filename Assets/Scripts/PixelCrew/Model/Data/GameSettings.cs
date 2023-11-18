using PixelCrew.Model.Data.Properties.Persistent;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }

        [SerializeField] private FloatPersistentProperty musicVolume;
        [SerializeField] private FloatPersistentProperty sfxVolume;

        public FloatPersistentProperty MusicVolume => musicVolume;
        public FloatPersistentProperty SfxVolume => sfxVolume;

        private void OnEnable()
        {
            musicVolume = new FloatPersistentProperty(1, SoundSetting.Music.ToString());
            sfxVolume = new FloatPersistentProperty(1, SoundSetting.Sfx.ToString());
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            
            EditorApplication.delayCall += () =>
            {
                musicVolume.Validate();
                sfxVolume.Validate();
            };
        }
#endif
        
    }

    public enum SoundSetting
    {
        Music,
        Sfx
    }
}