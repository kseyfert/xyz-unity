using PixelCrew.Components.Singletons;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckpointComponent : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private bool isDefaultCheckpoint = false;
        [SerializeField] private bool isChecked = false;
        
        [SerializeField] protected UnityEvent onChecked;
        [SerializeField] protected UnityEvent onUnchecked;

        public string Id => id;
        public bool IsDefault => isDefaultCheckpoint;
        public bool IsChecked => isChecked;
        
        private GameSessionSingleton _gameSession;
        private SpawnComponent _spawnComponent;

        protected virtual void Start()
        {
            _spawnComponent = GetComponent<SpawnComponent>();
            
            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            if (_gameSession.IsCheckpointChecked(Id)) Check();
            else Uncheck();
        }
        
        public void Check()
        {
            if (isChecked) return;
            
            isChecked = true;
            _gameSession.SetCheckpoint(Id, true);
            onChecked?.Invoke();
        }

        public void Uncheck()
        {
            if (!isChecked) return;
            
            isChecked = false;
            _gameSession.SetCheckpoint(Id, false);
            onUnchecked?.Invoke();
        }
        
        public void Switch()
        {
            isChecked = !isChecked;
        }
        
        public void SpawnHero()
        {
            if (_spawnComponent == null)
            {
                _spawnComponent = GetComponent<SpawnComponent>();
            }
            _spawnComponent.Spawn();
        }
    }
}