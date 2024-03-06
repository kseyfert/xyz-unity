using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Utils.ObjectPool
{
    public class ObjectPoolItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent onRestart;
        
        private int _id;
        private ObjectPoolSingleton _pool;

        private void Awake()
        {
            _pool = SingletonMonoBehaviour.GetInstance<ObjectPoolSingleton>();
        }

        public void Restart()
        {
            onRestart?.Invoke();
        }

        public void Release()
        {
            _pool.Release(_id, this);
        }
        
        public void Retain(int id)
        {
            _id = id;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}