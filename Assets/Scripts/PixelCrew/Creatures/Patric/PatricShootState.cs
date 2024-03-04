using PixelCrew.Components.Game;
using UnityEngine;

namespace PixelCrew.Creatures.Patric
{
    public class PatricShootState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularSpawnComponent>();
            spawner.Spawn();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularSpawnComponent>();
            spawner.Interrupt();
        }
    }
}
