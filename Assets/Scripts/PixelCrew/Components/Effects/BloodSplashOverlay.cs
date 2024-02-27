using System;
using PixelCrew.Components.Singletons;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.Effects
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {
        private static readonly int Health = Animator.StringToHash("Health");
        [SerializeField] private Transform overlay;

        private GameSessionSingleton _gameSession;
        private Animator _animator;
        private Vector3 _initialScale;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _initialScale = overlay.localScale - Vector3.one;
            
            overlay.gameObject.SetActive(false);

            _gameSession = SingletonMonoBehaviour.GetInstance<GameSessionSingleton>();
            if (_gameSession == null) return;
            
            _trash.Retain(_gameSession.Model.hp.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged(int oldValue, int newValue)
        {
            var maxHp = _gameSession.StatsModel.GetValue(StatId.Hp);
            var hpNormalized = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f);
            overlay.localScale = Vector3.one + _initialScale * overlayModifier;
            
            overlay.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}