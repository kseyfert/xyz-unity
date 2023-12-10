using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties.Persistent
{
    [Serializable]
    public class StringPersistentProperty : APrefsPersistentProperty<string>
    {
        public StringPersistentProperty(string defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        protected override string Read(string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }
}