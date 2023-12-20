using UnityEngine;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(Animator))]
    public class FlagComponent : CheckpointComponent
    {
        private readonly int _animatorKey = Animator.StringToHash("is-checked");

        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(_animatorKey, IsChecked);
        }
    }
}