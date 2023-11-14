using System;
using UnityEngine;

namespace PixelCrew.UI
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedWindow : MonoBehaviour
    {
        private static readonly int ShowTrigger = Animator.StringToHash("show");
        private static readonly int HideTrigger = Animator.StringToHash("hide");
        
        private Animator _animator;

        private void Start()
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
            Destroy(gameObject);
        }
    }
}