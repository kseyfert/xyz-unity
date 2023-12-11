using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties.Persistent;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.LanguageMenu
{
    public class LanguageMenu : AnimatedWindow
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject container;

        private readonly List<LanguageItem> _items = new List<LanguageItem>();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        protected override void Start()
        {
            base.Start();

            var locales = LocalizationManager.I.Index.Locales;
            foreach (var locale in locales)
            {
                var go = Instantiate(prefab, container.transform);
                go.SetActive(true);
                
                var languageItem = go.GetComponent<LanguageItem>();
                languageItem.SetLanguage(locale.Code, $"{locale.Code.ToUpper()} / {locale.Title.Capitalize()}");
                var disposable = languageItem.Subscribe(() =>
                {
                    _items
                        .FindAll(a => a != languageItem)
                        .ForEach(a => a.Deselect());
                    
                    LocalizationManager.I.SetLocale(locale.Code);
                });
                
                _items.Add(languageItem);
                _trash.Retain(disposable);
            }

            _trash.Retain(LocalizationManager.I.SubscribeAndInvoke(OnLocaleChanged));
        }

        private void OnLocaleChanged()
        {
            _items.ForEach(item => item.Deselect());
            var index = _items.FindIndex(item => item.Code == LocalizationManager.I.LocaleCode);
            if (index == -1) index = 0;
            _items[index].Select();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}