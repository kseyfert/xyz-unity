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
            Open("UI/SettingsWindow");
        }

        public void OnLanguage()
        {
            Open("UI/LanguageWindow");
        }

        public void OnStart()
        {
            _action = () => SceneManager.LoadScene("Level1");
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