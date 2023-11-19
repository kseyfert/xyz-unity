using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PropsDef", fileName = "PropsDef")]
    public class PropsDef : ScriptableObject
    {
        [SerializeField] private int potionPower;
        public int PotionPower => potionPower;
    }
}