using System;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties.Persistent
{
    [Serializable]
    public abstract class APersistentProperty<TPropertyType>
    {
        [Range(0f, 1f)]
        [SerializeField] private TPropertyType value;
        private TPropertyType _defaultValue;
        private TPropertyType _stored;

        public delegate void PropertyDelegate(TPropertyType oldValue, TPropertyType newValue);
        public event PropertyDelegate OnChanged;

        public IDisposable Subscribe(PropertyDelegate call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _stored;
                Write(value);
                _stored = this.value = value;
                
                OnChanged?.Invoke(oldValue, this.value);
            }
        }

        public APersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected void Init()
        {
            value = Read(_defaultValue);
        }

        public void Validate()
        {
            if (!_stored.Equals(value)) Value = value;
        }
        
        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}