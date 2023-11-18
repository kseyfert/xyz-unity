using System;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties.Observable
{
    [Serializable]
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType value;

        private Action<TPropertyType, TPropertyType> OnChanged = (a, b) => { };
        
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

        public ActionDisposable Subscribe(Action<TPropertyType, TPropertyType> call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public ActionDisposable SubscribeAndInvoke(Action<TPropertyType, TPropertyType> call)
        {
            call?.Invoke(value, value);
            return Subscribe(call);
        }
    }
}