using System;
using PixelCrew.UI.LevelLoader;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private LevelLoaderSingleton _levelLoader;
        private Action _action;

        protected override void Start()
        {
            base.Start();

            _levelLoader = SingletonMonoBehaviour.GetInstance<LevelLoaderSingleton>();
        }

        public void OnSettings()
        {
            Open("UI/SettingsWindow");
        }

        public void OnLanguage()
        {
            Open("UI/LanguageWindow");
        }

        public void OnStart()
        {
            _action = () => _levelLoader.Show("Level1");
            Close();
        }

        public void OnExit()
        {
            _action = () => Application.Quit();
#if UNITY_EDITOR                
            _action += () => UnityEditor.EditorApplication.isPlaying = false;
#endif

            Close();
        }

        public override void AnimationEventClose()
        {
            _action?.Invoke();
            base.AnimationEventClose();
        }
    }
}