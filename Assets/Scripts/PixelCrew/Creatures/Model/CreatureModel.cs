using System;
using System.Collections.Generic;
using PixelCrew.Creatures.Model.Data;
using PixelCrew.Model.Data.Properties.Observable;
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
        public static readonly string Keys = "Keys";
        public static readonly string Potions = "Potions";

        public InventoryData inventory = new InventoryData();

        public IntObservableProperty hp = new IntObservableProperty();
        
        public List<LevelPosition> lastPositions = new List<LevelPosition>();
    
        [Serializable]
        public struct LevelPosition
        {
            public string levelName;
            public Vector3 levelPosition;
        }
    }
}