using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.PauseMenu
{
    public class PauseMenuWindow : AnimatedWindow
    {
        private Action _action;

        private float _defaultTimeScale;

        protected override void Start()
        {
            base.Start();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnSettings()
        {
            Open("UI/SettingsWindow");
        }

        public void OnRestart()
        {
            var scene = SceneManager.GetActiveScene();
            _action = () => SceneManager.LoadScene(scene.name);
            Close();
        }

        public void OnExit()
        {
            _action = () => SceneManager.LoadScene("Scenes/MainMenu");
            Close();
        }

        public override void AnimationEventClose()
        {
            _action?.Invoke();
            base.AnimationEventClose();
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}