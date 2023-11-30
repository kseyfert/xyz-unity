using System;
using UnityEngine;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class DialogBoxContainerFacadeComponent : MonoBehaviour
    {
        [SerializeField] private DialogBoxContainerComponent center;
        [SerializeField] private DialogBoxContainerComponent left;
        [SerializeField] private DialogBoxContainerComponent right;

        private DialogBoxContainerComponent _activated;
        private bool _isActivated = false;

        private void Awake()
        {
            if (center != null) center.Deactivate();
            if (left != null) left.Deactivate();
            if (right != null) right.Deactivate();
        }

        public void Activate(bool personalized=false)
        {
            if (personalized && left != null) _activated = left;
            else if (personalized && right != null) _activated = right;
            else _activated = center;
            
            _activated.Activate();
            _isActivated = true;
        }
        
        public void Deactivate()
        {
            if (!_isActivated) return;
            
            _activated.Deactivate();
        }

        public bool IsActivated()
        {
            return _isActivated;
        }

        public void SetMessage(string message)
        {
            if (!_isActivated) return;
            
            _activated.SetMessage(message);
        }
        
        public string GetMessage()
        {
            if (!_isActivated) return string.Empty;
            
            return _activated.GetMessage();
        }

        public void SetAuthor(string author)
        {
            if (!_isActivated) return;
            
            if (_activated.GetAuthor() == author) return;
            
            _activated.SetAuthor(author);
            if (_activated == center) return;
            
            SwitchActivated();
        }

        public string GetAuthor()
        {
            if (!_isActivated) return string.Empty;
            
            return _activated.GetAuthor();
        }

        public void SetAvatar(Sprite avatar)
        {
            if (!_isActivated) return;
            
            _activated.SetAvatar(avatar);
        }

        public Sprite GetAvatar()
        {
            if (!_isActivated) return null;
            
            return _activated.GetAvatar();
        }

        private void SwitchActivated()
        {
            var oldActive = _activated;
            var newActive = _activated == left ? right : left;

            if (newActive == null) return;
            
            newActive.Activate();
            newActive.SetMessage(oldActive.GetMessage());
            newActive.SetAuthor(oldActive.GetAuthor());
            newActive.SetAvatar(oldActive.GetAvatar());
            
            oldActive.Deactivate();

            _activated = newActive;
        }

        public void SetEmpty()
        {
            if (!_isActivated) return;
            
            SetMessage(string.Empty);
            SetAuthor(string.Empty);
            SetAvatar(null);
        }
    }
}