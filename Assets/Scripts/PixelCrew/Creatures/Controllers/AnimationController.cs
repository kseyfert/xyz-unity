using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class AnimationController : AController
    {
        public static readonly int TriggerHit = Animator.StringToHash("hit");
        public static readonly int TriggerAttack = Animator.StringToHash("attack");
        public static readonly int TriggerThrow = Animator.StringToHash("throw");
        public static readonly int TriggerThrowMax = Animator.StringToHash("throw-max");

        public static readonly int BoolIsGrounded = Animator.StringToHash("is-grounded");
        public static readonly int BoolIsRunning = Animator.StringToHash("is-running");
        public static readonly int BoolIsDoubleJumping = Animator.StringToHash("is-double-jumping");
        public static readonly int BoolIsDead = Animator.StringToHash("is-dead");

        public static readonly int FloatVelocityY = Animator.StringToHash("velocity-y");

        [SerializeField] private Creature creature;
        
        [Serializable]
        public struct AnimationProfile
        {
            public string name;
            public bool isDefault;
            public RuntimeAnimatorController controller;
        }
        [SerializeField] private List<AnimationProfile> animationProfiles;

        private Animator _animator;
        private readonly Dictionary<int, Func<bool>> _boolActions = new Dictionary<int, Func<bool>>();
        private readonly Dictionary<int, Func<float>> _floatActions = new Dictionary<int, Func<float>>();

        private string _currentProfileName = "";

        private void Start()
        {
            _animator = creature.Animator;

            if (_currentProfileName == "")
            {
                var index = animationProfiles.FindIndex((item) => item.isDefault);
                index = index >= 0 ? index : 0;
                SetProfile(animationProfiles[index].name);    
            }
            else
            {
                SetProfile(_currentProfileName);
            }
        }

        public void SetTrigger(int key)
        {
            _animator.SetTrigger(key);
        }

        public void SetBoolUpdate(int key, Func<bool> action)
        {
            _boolActions[key] = action;
        }

        public void SetFloatUpdate(int key, Func<float> action)
        {
            _floatActions[key] = action;
        }

        private void Update()
        {
            foreach (var pair in _boolActions)
            {
                var action = pair.Value;
                var v = action?.Invoke();
                if (v.HasValue) _animator.SetBool(pair.Key, v.Value);
            }

            foreach (var pair in _floatActions)
            {
                var action = pair.Value;
                var v = action?.Invoke();
                
                if (v.HasValue) _animator.SetFloat(pair.Key, v.Value);
            }
        }

        public void SetProfile(string profileName)
        {
            _currentProfileName = profileName;
            if (_animator == null) return;
            
            var index = animationProfiles.FindIndex(item => item.name == profileName);
            if (index < 0) return;
            
            _animator.runtimeAnimatorController = animationProfiles[index].controller;
        }

        protected override Creature GetCreature()
        {
            return creature;
        }
    }
}