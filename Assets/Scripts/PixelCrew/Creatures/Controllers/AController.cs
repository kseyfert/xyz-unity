using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public abstract class AController : MonoBehaviour
    {
        [SerializeField] private Creature creature;

        public Creature Creature => creature;
        
        public virtual void Die()
        {
            if (creature.gameObject != gameObject) gameObject.SetActive(false);
            else enabled = false;
        }
    }
}