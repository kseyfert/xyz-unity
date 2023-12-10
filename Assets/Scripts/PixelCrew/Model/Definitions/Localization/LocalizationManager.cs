using System;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public static readonly LocalizationManager I;
        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        private LocaleIndexDef _index;
        private string _currentLocaleCode;
        private LocaleDef _currentLocale;

        private Action _onLocaleChanged = delegate { };

        public LocaleIndexDef Index => _index;
        public LocaleDef Locale => _currentLocale;
        public string LocaleCode => _currentLocaleCode;

        public LocalizationManager()
        {
            _index = Resources.Load<LocaleIndexDef>($"Locales/_index");
            GameSettings.I.Locale.Subscribe(OnSettingChanged);
            LoadLocale();
        }

        public void SetLocale(string code)
        {
            if (string.IsNullOrEmpty(code)) return;

            var found = Array.FindIndex(_index.Codes, item => item == code);
            if (found == -1) return;

            GameSettings.I.Locale.Value = code;
        }

        private void OnSettingChanged(string oldValue, string newValue)
        {
            LoadLocale();
        }

        private void LoadLocale()
        {
            var code = GameSettings.I.Locale.Value;
            
            _currentLocale = Resources.Load<LocaleDef>($"Locales/{code}");
            _currentLocaleCode = code;
            
            _onLocaleChanged?.Invoke();
        }

        public IDisposable Subscribe(Action call)
        {
            _onLocaleChanged += call;
            return new ActionDisposable(() => _onLocaleChanged -= call);
        }

        public IDisposable SubscribeAndInvoke(Action call)
        {
            call?.Invoke();
            return Subscribe(call);
        }
    }
}