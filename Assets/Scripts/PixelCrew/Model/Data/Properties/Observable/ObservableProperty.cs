using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties.Observable
{
    [Serializable]
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType value;

        public delegate void PropertyDelegate(TPropertyType oldValue, TPropertyType newValue);
        public event PropertyDelegate OnChanged;

        public TPropertyType Value
        {
            get => value;
            set
            {
                var isSame = this.value.Equals(value);
                if (isSame) return;
                var oldValue = this.value;

                this.value = value;
                OnChanged?.Invoke(oldValue, this.value);
            }
        }
    }
}