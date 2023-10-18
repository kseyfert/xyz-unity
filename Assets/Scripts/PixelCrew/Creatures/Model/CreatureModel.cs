using System;
using System.Collections.Generic;
using PixelCrew.Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace PixelCrew.Creatures.Model
{
    [Serializable]
    public class CreatureModel
    {
        public static readonly string Coins = "Coins";
        public static readonly string Swords = "Swords";
        public static readonly string Weapons = "Weapons";

        public InventoryData inventory = new InventoryData();

        public int hp = 100;
        public int currentStock = 0;
        public bool isDoubleJumpAllowed = false;
        public bool isInfiniteJumpAllowed = false;
        public List<LevelPosition> lastPositions = new List<LevelPosition>();
    
        [Serializable]
        public struct LevelPosition
        {
            public string levelName;
            public Vector3 levelPosition;
        }
    }
}