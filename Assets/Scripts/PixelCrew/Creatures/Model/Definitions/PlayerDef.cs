using UnityEngine;

namespace PixelCrew.Creatures.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;
    }
}