using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public abstract class AController : MonoBehaviour
    {
        protected abstract Creature GetCreature();
        
        public virtual void Die()
        {
            if (GetCreature().gameObject != gameObject) gameObject.SetActive(false);
            else enabled = false;
        }
    }
}