using System;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [Serializable]
    public struct StatDef
    {
        [SerializeField] private StatId id;
        [SerializeField] private string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private StatLevel[] levels;

        public StatId Id => id;
        public string Name => name;
        public Sprite Icon => icon;
        public StatLevel[] Levels => levels;
    }

    [Serializable]
    public struct StatLevel
    {
        [SerializeField] private float value;
        [SerializeField] private ItemWithCount price;

        public float Value => value;
        public ItemWithCount Price => price;
    }

    [Serializable]
    public enum StatId
    {
        Hp,
        Speed,
        RangeDamage
    }
}