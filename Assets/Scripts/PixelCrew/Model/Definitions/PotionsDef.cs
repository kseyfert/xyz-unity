using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PotionsDef", fileName = "PotionsDef")]
    public class PotionsDef : ScriptableObject
    {
        [SerializeField] private PotionDef[] items;

        public bool Has(string id)
        {
            return Array.FindIndex(items, item => item.Id == id) > -1;
        }
        
        public PotionDef Get(string id)
        {
            return Array.Find(items, item => item.Id == id);
        }
    }

    [Serializable]
    public struct PotionDef
    {
        [InventoryId] 
        [SerializeField] private string id;
        [SerializeField] private PotionType type;
        [SerializeField] private int power;

        public string Id => id;
        public PotionType Type => type;
        public int Power => power;
    }

    [Serializable]
    public enum PotionType
    {
        Health,
        Speed
    }
}