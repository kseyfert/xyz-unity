using System;
using PixelCrew.Utils.Disposables;
using PixelCrew.Utils.Disposables.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PixelCrew.Components.Utils
{
    public class ClickComponent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UnityEvent action;
        public void OnPointerClick(PointerEventData eventData)
        {
            action?.Invoke();
        }
        
        public ActionDisposable Subscribe(Action call)
        {
            var disposable = (ActionDisposable)action.Subscribe(call);
            return disposable;
        }

        public ActionDisposable SubscribeAndInvoke(Action call)
        {
            call?.Invoke();
            return Subscribe(call);
        }
    }
}