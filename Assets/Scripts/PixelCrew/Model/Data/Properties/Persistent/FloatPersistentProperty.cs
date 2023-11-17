using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties.Persistent
{
    [Serializable]
    public class FloatPersistentProperty : APrefsPersistentProperty<float>
    {
        public FloatPersistentProperty(float defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }
    }
}