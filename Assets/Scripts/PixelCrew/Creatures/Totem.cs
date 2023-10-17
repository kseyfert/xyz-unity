using PixelCrew.Creatures.Controllers;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Totem : Creature
    {
        [SerializeField] private Totem topTotem;
        protected override void Init()
        {
            var animationController = base.AnimationController;
            var attackController = base.AttackController;
            var particlesController = base.ParticlesController;
            var healthController = base.HealthController;
            
            attackController.onThrowStarted += () => animationController.SetTrigger(AnimationController.TriggerThrow);
            attackController.onThrowFinished += () => particlesController.Spawn("sword-thrown");
            
            attackController.onThrowMaxStarted += () => animationController.SetTrigger(AnimationController.TriggerThrowMax);
            attackController.onThrowMaxFinished += (count, timeout) => particlesController.SpawnSeq("sword-thrown", count, timeout);

            healthController.onDamage += () => animationController.SetTrigger(AnimationController.TriggerHit);
            healthController.onDie += () => animationController.SetTrigger(AnimationController.TriggerHit);
            
            animationController.SetBoolUpdate(AnimationController.BoolIsDead, () => healthController.GetHealthComponent().IsDead());
            
            if (topTotem != null) animationController.SetProfile("middle");
            else animationController.SetProfile("top");
        }
    }
}