using PixelCrew.Components.Game;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private ProgressBarWidget progressBar;
        [SerializeField] private CanvasGroup canvasGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(healthComponent.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged()
        {
            var currentHp = healthComponent.GetCurrentHealth();
            var maxHp = healthComponent.GetMaxHealth();
            var progress = 1f * currentHp / maxHp;

            if (currentHp == 0) HideUI();
            else progressBar.SetProgress(progress);
        }

        public void ShowUI()
        {
            this.DoLerp(0, 1, 1, alpha => canvasGroup.alpha = alpha);
        }

        public void HideUI()
        {
            this.DoLerp(1, 0, 1, alpha => canvasGroup.alpha = alpha);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}