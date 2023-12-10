using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/Locale", fileName = "Locale")]
    public class LocaleDef : ScriptableObject
    {
        // en - https://docs.google.com/spreadsheets/d/e/2PACX-1vSo3l0JcCaZLuTQr6BCc32EN4SwdoxMorGkE0pEUoXkoUTQQBATQFEwv7S60bLCQYwP_Zfj3UMLzN1v/pub?gid=0&single=true&output=tsv
        // ru - https://docs.google.com/spreadsheets/d/e/2PACX-1vSo3l0JcCaZLuTQr6BCc32EN4SwdoxMorGkE0pEUoXkoUTQQBATQFEwv7S60bLCQYwP_Zfj3UMLzN1v/pub?gid=1919146577&single=true&output=tsv
        // fr - https://docs.google.com/spreadsheets/d/e/2PACX-1vSo3l0JcCaZLuTQr6BCc32EN4SwdoxMorGkE0pEUoXkoUTQQBATQFEwv7S60bLCQYwP_Zfj3UMLzN1v/pub?gid=627395834&single=true&output=tsv

        [SerializeField] private string url;
        [SerializeField] private List<LocaleItem> items = new List<LocaleItem>();

        public string[] Keys => items.ConvertAll(item => item.Key).ToArray();
        public string this[string key] => items.Find(item => item.Key == key).Value;

        private UnityWebRequest _request;

        [ContextMenu("Update Locale")]
        private void UpdateLocale()
        {
            if (_request != null) return;

            _request = UnityWebRequest.Get(url);
            
            var async = _request.SendWebRequest();
            async.completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (!operation.isDone) return;
            operation.completed -= OnDataLoaded;
            
            items.Clear();

            var data = _request.downloadHandler.text;
            
            var rows = data.Split('\n');
            rows = Array.ConvertAll(rows, item => item.Trim());
            rows = Array.FindAll(rows, item => !string.IsNullOrWhiteSpace(item));
            
            foreach (var row in rows)
            {
                try
                {
                    var split = row.Split('\t');
                    var newItem = new LocaleItem(split[0], split[1]);
                    items.Add(newItem);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Cannot parse locale row: {row}, err: {e}");
                }
            }

            
            _request = null;
        }

        [Serializable]
        private class LocaleItem
        {
            [SerializeField] private string key;
            [SerializeField] private string value;

            public string Key => key;
            public string Value => value;

            public LocaleItem(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}