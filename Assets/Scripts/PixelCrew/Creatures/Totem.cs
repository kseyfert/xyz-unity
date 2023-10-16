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
            
            attackController.OnThrowStarted += (obj, args) => animationController.SetTrigger(AnimationController.TriggerThrow);
            attackController.OnThrowFinished += (obj, args) => particlesController.Spawn("sword-thrown");
            
            attackController.OnThrowMaxStarted += (obj, args) => animationController.SetTrigger(AnimationController.TriggerThrowMax);
            attackController.OnThrowMaxFinished += (obj, args) =>
            {
                var a = (AttackController.ThrowMaxEventArgs)args;
                particlesController.SpawnSeq("sword-thrown", a.count, a.timeout);
            };

            healthController.OnDamage += (obj, args) => animationController.SetTrigger(AnimationController.TriggerHit);
            healthController.OnDie += (obj, args) => animationController.SetTrigger(AnimationController.TriggerHit);
            
            animationController.SetBoolUpdate(AnimationController.BoolIsDead, () => healthController.GetHealthComponent().IsDead());
            
            if (topTotem != null) animationController.SetProfile("middle");
            else animationController.SetProfile("top");
        }
    }
}