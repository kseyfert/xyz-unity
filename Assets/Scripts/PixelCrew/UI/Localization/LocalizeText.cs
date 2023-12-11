using System;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : MonoBehaviour
    {
        [SerializeField] private string key;

        private CompositeDisposable _trash = new CompositeDisposable();
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            _trash.Retain(LocalizationManager.I.SubscribeAndInvoke(OnLocaleChanged));
        }

        private void OnLocaleChanged()
        {
            _text.text = LocalizationManager.I.Locale[key];
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}