using PixelCrew.Creatures.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Creatures.Components
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [InventoryId] 
        [SerializeField] private string id;
        [SerializeField] private int defaultCount;

        public void Apply(GameObject go, int count)
        {
            var creature = go.GetComponent<Creature>();
            if (creature == null) return;

            var sessionController = creature.SessionController;
            if (sessionController == null) return;

            var inventory = sessionController.GetModel().inventory;
            inventory?.Add(id, count);
        }

        public void Apply(GameObject go)
        {
            Apply(go, defaultCount);
        }
    }
}