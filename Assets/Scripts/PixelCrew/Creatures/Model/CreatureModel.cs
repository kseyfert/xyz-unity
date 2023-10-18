using System;
using System.Collections.Generic;
using PixelCrew.Creatures.Model.Data;
using UnityEngine;

namespace PixelCrew.Creatures.Model
{
    [Serializable]
    public class CreatureModel
    {
        public static readonly string Coins = "Coins";
        public static readonly string Weapons = "Weapons";
        public static readonly string DoubleJumper = "DoubleJumper";
        public static readonly string InfiniteJumper = "InfiniteJumper";

        public InventoryData inventory = new InventoryData();

        public int hp = 100;
        
        public List<LevelPosition> lastPositions = new List<LevelPosition>();
    
        [Serializable]
        public struct LevelPosition
        {
            public string levelName;
            public Vector3 levelPosition;
        }
    }
}