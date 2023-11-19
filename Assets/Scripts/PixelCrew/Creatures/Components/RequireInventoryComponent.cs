using System;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Components
{
    public class RequireInventoryComponent : MonoBehaviour
    {
        [InventoryId] 
        [SerializeField] private string id;
        [SerializeField] private int atLeast = 1;
        [SerializeField] private bool removeAfterUse = false;

        [SerializeField] private RequireEvent onSuccess;
        [SerializeField] private RequireEvent onFail;

        public void Check(GameObject go)
        {
            var creature = go.GetComponent<Creature>();
            if (creature == null) return;

            var sessionController = creature.SessionController;
            if (sessionController == null) return;

            var inventory = sessionController.GetModel().inventory;
            if (inventory == null) return;
            
            if (inventory.Has(id, atLeast))
            {
                onSuccess?.Invoke(go);
                if (removeAfterUse) inventory.Remove(id, atLeast);
            }
            else onFail?.Invoke(go);
        }
     
        
        [Serializable]
        public class RequireEvent : UnityEvent<GameObject> {}
    }
}