using System;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.LanguageMenu
{
    public class LanguageItem : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private GameObject selected;
        
        private Action OnSelected;
        private string _code;
        private bool _isSelected;

        public string Code => _code;

        public void SetLanguage(string code, string title)
        {
            _code = code;
            titleText.text = title.Capitalize();
        }

        public string GetLanguageCode()
        {
            return _code;
        }

        public bool IsSelected()
        {
            return _isSelected;
        }

        public void Select()
        {
            if (IsSelected()) return;
            
            _isSelected = true;
            selected.SetActive(true);
            OnSelected?.Invoke();
        }

        public void Deselect()
        {
            if (!IsSelected()) return;

            _isSelected = false;
            selected.SetActive(false);
        }

        public void Switch()
        {
            if (IsSelected()) Deselect();
            else Select();
        }
        
        public ActionDisposable Subscribe(Action call)
        {
            OnSelected += call;
            return new ActionDisposable(() => OnSelected -= call);
        }

        public ActionDisposable SubscribeAndInvoke(Action call)
        {
            call?.Invoke();
            return Subscribe(call);
        }
    }
}