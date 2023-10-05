using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelCrew.Controllers
{
    public class CheatController : MonoBehaviour
    {
        [SerializeField] private float inputLifetime = 2;
        [SerializeField] private Cheat[] cheats;

        private string _currentInput = "";
        private float _inputTimer = 0;

        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnTextInput(char ch)
        {
            _currentInput += ch;
            _currentInput = _currentInput.ToLower();
            _inputTimer = inputLifetime;
        }

        private void Update()
        {
            if (_inputTimer < 0)
            {
                _currentInput = string.Empty;
            }
            else
            {
                _inputTimer -= Time.deltaTime;
            }

            foreach (Cheat cheat in cheats)
            {
                if (cheat == null) continue;
                if (cheat.code == "") continue;
                
                if (_currentInput.Contains(cheat.code.ToLower()))
                {
                    cheat.action?.Invoke();
                    _currentInput = string.Empty;
                }
            }
        }
        
        [Serializable]
        private class Cheat
        {
            [SerializeField] public string name;
            [SerializeField] public string code;
            [SerializeField] public UnityEvent action;
        }
    }
}