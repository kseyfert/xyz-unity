using PixelCrew.Hero;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero.Hero>();
            if (hero == null) return;

            var ac = go.GetComponentInChildren<AttackController>();
            ac.Arm();
        }
        
    }
}