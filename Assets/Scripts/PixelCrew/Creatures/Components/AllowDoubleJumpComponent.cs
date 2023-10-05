using UnityEngine;

namespace PixelCrew.Creatures.Components
{
    public class AllowDoubleJumpComponent : MonoBehaviour
    {
        public void Allow(GameObject go)
        {
            var creature = go.GetComponent<Creature>();
            if (creature == null) return;

            var mc = creature.MovementController;
            mc.AllowDoubleJump();
        }
    }
}