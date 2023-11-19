using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItemsDef", fileName = "ThrowableItemsDef")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] items;

        public ThrowableDef Get(string id)
        {
            return Array.Find(items, item => item.Id == id);
        }
    }

    [Serializable]
    public struct ThrowableDef
    {
        [InventoryId] 
        [SerializeField] private string id;
        [SerializeField] private bool allowThrowLast;
        [SerializeField] private GameObject projectile;

        public string Id => id;
        public GameObject Projectile => projectile;
        public bool AllowThrowLast => allowThrowLast;
    }
}