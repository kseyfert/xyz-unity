using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class DialogBoxContainerComponent : MonoBehaviour
    {
        [SerializeField] private Text message;
        [SerializeField] private Image avatar;
        [SerializeField] private Text author;

        private bool _hasMessage;
        private bool _hasAvatar;
        private bool _hasAuthor;

        private string _currentAuthor;

        private void Awake()
        {
            _hasMessage = message != null;
            _hasAuthor = author != null;
            _hasAvatar = avatar != null;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
        
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
        
        public void SetMessage(string text)
        {
            if (!_hasMessage) return;
            message.text = text;
        }
        
        public string GetMessage()
        {
            if (!_hasMessage) return string.Empty;
            return message.text;
        }

        public void SetAuthor(string s)
        {
            _currentAuthor = s;
            
            if (!_hasAuthor) return;
            author.text = s;
        }

        public string GetAuthor()
        {
            return _currentAuthor;
        }

        public void SetAvatar(Sprite sprite)
        {
            if (!_hasAvatar) return;
            avatar.sprite = sprite;
        }

        public Sprite GetAvatar()
        {
            if (!_hasAvatar) return null;
            return avatar.sprite;
        }
    }
}