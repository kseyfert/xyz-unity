using System;
using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleIndex", fileName = "_index")]
    public class LocaleIndexDef : ScriptableObject
    {
        [SerializeField] private List<LocaleDefItem> locales = new List<LocaleDefItem>();

        public string[] Codes => locales.ConvertAll(item => item.Code).ToArray();
        public string[] Titles => locales.ConvertAll(item => item.Title).ToArray();
        public string[] Names => locales.ConvertAll(item => $"{item.Code} / {item.Title.Capitalize()}").ToArray();

        public LocaleDefItem[] Locales => locales.ToArray();
        public LocaleDefItem this[string code] => locales.Find(item => item.Code == code);
        

        [Serializable]
        public class LocaleDefItem
        {
            [SerializeField] private string code;
            [SerializeField] private string title;

            public string Code => code;
            public string Title => title;
        }
    }
}