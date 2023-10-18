using UnityEngine;

namespace PixelCrew.Creatures.Components
{
    public class ArmComponent : MonoBehaviour
    {
        [SerializeField] private int stock = 1;
        public void Arm(GameObject go)
        {
            var creature = go.GetComponent<Creature>();
            if (creature == null) return;

            var ac = creature.AttackController;
            ac.TakeWeapon(stock);
        }
    }
}