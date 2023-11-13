using PixelCrew.Controllers;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Crabby : Creature
    {
        [SerializeField] private AIController aiController;
        public override void AnimationEventRange()
        {
            if (AttackController == null) return;
            if (aiController == null) return;
            if (ParticlesController == null) return;

            var target = aiController.GetTarget();
            if (target == null) return;
            
            ParticlesController.SpawnAt(Controllers.AttackController.RangeParticle, target.transform.position);
            
            AttackController.onRangeApplied?.Invoke();
        }
    }
}