using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Settings
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget musicVolume;
        [SerializeField] private AudioSettingsWidget sfxVolume;

        protected override void Start()
        {
            base.Start();
            
            musicVolume.SetModel(GameSettings.I.MusicVolume);
            sfxVolume.SetModel(GameSettings.I.SfxVolume);
        }
    }
}