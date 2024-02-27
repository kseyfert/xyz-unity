using System;
using PixelCrew.UI.LevelLoader;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.PauseMenu
{
    public class PauseMenuWindow : AnimatedWindow
    {
        private LevelLoaderSingleton _levelLoader;
        private Action _action;

        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
            
            _levelLoader = SingletonMonoBehaviour.GetInstance<LevelLoaderSingleton>();
        }

        public void OnSettings()
        {
            Open("UI/SettingsWindow");
        }

        public void OnRestart()
        {
            var scene = SceneManager.GetActiveScene();
            _action = () => _levelLoader.Show(scene.name);
            Close();
        }

        public void OnExit()
        {
            _action = () => _levelLoader.Show("Scenes/MainMenu");
            Close();
        }

        public override void AnimationEventClose()
        {
            _action?.Invoke();
            base.AnimationEventClose();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Time.timeScale = _defaultTimeScale;
        }
    }
}