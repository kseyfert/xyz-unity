using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Patric
{
    public class BossNextStageState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularSpawnComponent>();
            if (spawner != null) spawner.NextProfile();

            var lightChanger = animator.GetComponent<ChangeGlobalLightComponent>();
            if (lightChanger != null) lightChanger.ApplyLerp();
        }
    }
}
