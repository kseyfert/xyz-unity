using PixelCrew.Components.Game;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Creatures.Patric
{
    public class HealthAnimationConnect : MonoBehaviour
    {
        private static readonly int Health = Animator.StringToHash("health");

        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private Animator animator;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _trash.Retain(healthComponent.SubscribeAndInvoke(OnHealthChanged));
        }

        private void OnHealthChanged()
        {
            animator.SetFloat(Health, healthComponent.GetCurrentHealth());
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}