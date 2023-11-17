using System;
using UnityEngine;

namespace PixelCrew.UI
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedWindow : MonoBehaviour
    {
        private static readonly int ShowTrigger = Animator.StringToHash("show");
        private static readonly int HideTrigger = Animator.StringToHash("hide");

        public delegate void AnimatedWindowDelegate(AnimatedWindow window);
        public AnimatedWindowDelegate onClosed;
        
        private Animator _animator;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(ShowTrigger);
        }

        public void Close()
        {
            _animator.SetTrigger(HideTrigger);
        }

        public virtual void AnimationEventClose()
        {
            onClosed?.Invoke(this);
            Destroy(gameObject);
        }

        public static AnimatedWindow Open(string path)
        {
            var window = Resources.Load<GameObject>(path);
            if (window == null) return null;

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return null;

            var go = Instantiate(window, canvas.transform);
            return go.GetComponent<AnimatedWindow>();
        }

        public static void CloseAll()
        {
            var windows = FindObjectsOfType<AnimatedWindow>();
            foreach (var window in windows)
            {
                window.Close();
            }
        }
    }
}