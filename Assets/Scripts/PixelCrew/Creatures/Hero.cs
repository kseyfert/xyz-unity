using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        protected override void Init()
        {
            base.Init();

            // var hc = HealthController.GetHealthComponent();
            // hc.SetMaxHealth(DefsFacade.I.Player.MaxHealth);
        }
    }
}