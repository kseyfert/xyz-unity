using UnityEngine;

namespace PixelCrew.Creatures.Components
{
    public class InverseComponent : MonoBehaviour
    {
        public void Inverse(GameObject go)
        {
            var creature = go.GetComponent<Creature>();
            if (creature == null) return;

            var mc = creature.MovementController;
            if (mc == null) return;
            
            mc.Inverse();
        }
    }
}