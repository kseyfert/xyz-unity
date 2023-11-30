using System;
using System.Collections;
using PixelCrew.Components.Game;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Hud.Dialogs
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlaySoundComponent))]
    public class DialogBoxSingleton : SingletonMonoBehaviour
    {
        private static readonly string SoundOpen = "open";
        private static readonly string SoundClose = "close";
        private static readonly string SoundTyping = "typing";
        private static readonly int AnimationIsOpen = Animator.StringToHash("is-open");

        [SerializeField] private DialogBoxContainerFacadeComponent container;
        
        [Space]
        [SerializeField] private float textSpeed = 0.09f;

        private PlaySoundComponent _playSoundComponent;
        private Animator _animator;

        private Coroutine _typingCoroutine;

        private DialogData _data;
        private Action _onClose;
        private int _currentPhraseIndex = -1;

        private void Awake()
        {
            Load<DialogBoxSingleton>();
            
            _playSoundComponent = GetComponent<PlaySoundComponent>();
            _animator = GetComponent<Animator>();

            container.Deactivate();
        }

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentPhraseIndex = -1;

            container.Activate(data.IsPersonalized());
            container.SetEmpty();
            NextPhrase();

            _playSoundComponent.Play(SoundOpen);
            _animator.SetBool(AnimationIsOpen, true);
        }

        public void ShowDialog(DialogData data, Action onClose)
        {
            _onClose = onClose;
            ShowDialog(data);
        }

        public void CloseDialog()
        {
            _playSoundComponent.Play(SoundClose);
            _animator.SetBool(AnimationIsOpen, false);
        }

        private void AnimationEventShowed()
        {
            // NextPhrase();
        }

        private void AnimationEventClosed()
        {
            _onClose?.Invoke();
            container.Deactivate();
        }

        public void OnButtonClicked()
        {
            if (_typingCoroutine != null) Skip();
            else if (HasNextPhrase()) NextPhrase();
            else CloseDialog();
        }
        
        private bool HasNextPhrase()
        {
            var index = _currentPhraseIndex + 1;
            return index >= 0 && index < _data.Phrases.Count;
        }

        private void NextPhrase()
        {
            _currentPhraseIndex++;
            if (_currentPhraseIndex >= _data.Phrases.Count) return;

            var phrase = _data.Phrases[_currentPhraseIndex];
            if (phrase.Avatar != null) container.SetAvatar(phrase.Avatar);
            if (phrase.Author != null) container.SetAuthor(phrase.Author);
            
            StartTyping();
        }

        private void Skip()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;    
            }
            if (_currentPhraseIndex >= _data.Phrases.Count) return;
            
            container.SetMessage(_data.Phrases[_currentPhraseIndex].Message);
        }

        private void StartTyping()
        {
            if (_currentPhraseIndex >= _data.Phrases.Count) return;
            
            _typingCoroutine = StartCoroutine(TypeText(_data.Phrases[_currentPhraseIndex].Message));
        }

        private IEnumerator TypeText(string message)
        {
            container.SetMessage(string.Empty);
            
            foreach (var ch in message)
            {
                _playSoundComponent.Play(SoundTyping);

                var msg = container.GetMessage();
                msg += ch;
                container.SetMessage(msg);
                
                yield return new WaitForSeconds(textSpeed);
            }

            _typingCoroutine = null;
        }

        [ContextMenu("Test-Open")]
        private void TestOpen()
        {
            var d = new DialogData(
                new[] {"AAAAALorem ipsum dolor sit amet", "Crabby"},
                new[] {"Hi...", "Captain"}, 
                new[] {"This is Kesha...", "Crabby"}, 
                new[] {"how Are you???", "Captain"}
            );
            ShowDialog(d);
        }
        
        [ContextMenu("Test-Close")]
        private void TestClose()
        {
            CloseDialog();
        }
    }
}