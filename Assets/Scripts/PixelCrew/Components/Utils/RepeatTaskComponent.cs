using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils
{
    public class RepeatTaskComponent : MonoBehaviour
    {
        [SerializeField] private float delay = 1f;
        [SerializeField] private GameObjectEvent action;

        private GameObject param = null;
        private bool _started = false;
        private Coroutine _coroutine;

        [ContextMenu("Start Task")]
        public void StartTask(GameObject go=null)
        {
            if (_started) return;

            param = go;
            _started = true;
            Worker();
        }
        
        [ContextMenu("Stop Task")]
        public void StopTask()
        {
            if (!_started) return;
            
            _started = false;
            param = null;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private void Worker()
        {
            if (!_started) return;
            
            action?.Invoke(param);
            _coroutine = this.SetTimeout(Worker, delay);
        }
        
        [Serializable]
        private class GameObjectEvent : UnityEvent<GameObject> {}
    }
}