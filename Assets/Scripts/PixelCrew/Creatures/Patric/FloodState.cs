using UnityEngine;

namespace PixelCrew.Creatures.Patric
{
    public class FloodState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var floodController = animator.GetComponent<FloodController>();
            if (floodController == null) return;
            
            floodController.DoFlooding();
        }
    }
}