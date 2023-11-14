using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _action;
        
        public void OnSettings()
        {
            var window = Resources.Load<GameObject>("UI/SettingsWindow");
            if (window == null) return;

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            Instantiate(window, canvas.transform);
        }

        public void OnStart()
        {
            var scene = SceneManager.GetActiveScene();
            // _action = () => SceneManager.LoadScene("Level1");
            _action = () => SceneManager.LoadScene(scene.name);
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