using PixelCrew.Creatures.Controllers;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Totem : Creature
    {
        [SerializeField] private bool top = true;
        protected override void Init()
        {
            var animationController = base.AnimationController;
            var attackController = base.AttackController;
            var healthController = base.HealthController;
            var soundController = base.SoundController;
            
            attackController.onRangeRequested += () => animationController.SetTrigger(AnimationController.TriggerThrow);
            attackController.onRangeMaxRequested += () => animationController.SetTrigger(AnimationController.TriggerThrowMax);

            healthController.onDamage += () => animationController.SetTrigger(AnimationController.TriggerHit);
            healthController.onDie += () => animationController.SetTrigger(AnimationController.TriggerHit);
            
            animationController.SetBoolUpdate(AnimationController.BoolIsDead, () => healthController.GetHealthComponent().IsDead());

            animationController.SetProfile(top ? "top" : "middle");

            if (soundController != null)
            {
                healthController.onDamage += () => soundController.Play("hit");
                healthController.onDie += () => soundController.Play("die");
                attackController.onRangeRequested += () => soundController.Play("range");
            }
        }
    }
}