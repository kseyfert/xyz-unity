using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private StatDef[] stats;
        public int MaxHealth => maxHealth;
        public StatDef[] Stats => stats;

        public StatDef GetStat(StatId id) => stats.FirstOrDefault(item => item.Id == id);
        public StatDef GetStat(string id) => stats.FirstOrDefault(item => item.Id.ToString() == id);
    }
}