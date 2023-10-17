using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class SandboxComponent : MonoBehaviour
    {
        public delegate void Test();

        private Test _onTest1;

        private Cooldown _cooldown1 = new Cooldown(1);
        private Cooldown _cooldown2 = new Cooldown(5);

        private void Start()
        {
            _onTest1 += () => Debug.Log("HI1");
            _onTest1 += () => Debug.Log("HI2");
            _onTest1 += () => Debug.Log("HI3");

            _cooldown1.Reset();
            _cooldown2.Reset();
        }

        private void Update()
        {
            if (_cooldown1.IsReady)
            {
                _onTest1?.Invoke();
                _cooldown1.Reset();
            }

            if (_cooldown2.IsReady)
            {
                _onTest1 = delegate {  };
                Debug.Log("Clearing listeners");
                Delegate.RemoveAll(_onTest1, _onTest1);
                // this.UnsubscribeAll(_onTest1);
                // foreach (var listener in _onTest1.GetInvocationList())
                // {
                //     _onTest1 -= (Test) listener;
                // }
                _cooldown2.Reset();
            }
        }
    }
}