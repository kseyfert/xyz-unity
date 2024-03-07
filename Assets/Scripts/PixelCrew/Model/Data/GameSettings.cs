using PixelCrew.Model.Data.Properties.Persistent;
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
        [SerializeField] private StringPersistentProperty locale;

        public FloatPersistentProperty MusicVolume => musicVolume;
        public FloatPersistentProperty SfxVolume => sfxVolume;
        public StringPersistentProperty Locale => locale;

        private void OnEnable()
        {
            musicVolume = new FloatPersistentProperty(1, GameSettingsKeys.Music.ToString());
            sfxVolume = new FloatPersistentProperty(1, GameSettingsKeys.Sfx.ToString());
            locale = new StringPersistentProperty("en", GameSettingsKeys.Locale.ToString());
            // PlayerPrefs.DeleteKey(GameSettingsKeys.Locale.ToString());
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                musicVolume.Validate();
                sfxVolume.Validate();
                locale.Validate();
            };
        }
#endif
        
    }

    public enum GameSettingsKeys
    {
        Music,
        Sfx,
        Locale
    }
}