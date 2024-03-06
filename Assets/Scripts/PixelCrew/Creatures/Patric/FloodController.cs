using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Patric
{
    public class FloodController : MonoBehaviour
    {
        private static readonly int IsFlooding = Animator.StringToHash("is-flooding");
        
        [SerializeField] private Animator animator;
        [SerializeField] private float floodTime;

        private bool _isFlooding = false;

        [ContextMenu("Do Flooding")]
        public void DoFlooding()
        {
            if (_isFlooding) return;
            
            _isFlooding = true;
            animator.SetBool(IsFlooding, true);

            this.SetTimeout(() =>
            {
                _isFlooding = false;
                animator.SetBool(IsFlooding, false);
            }, floodTime);
        }

    }
}