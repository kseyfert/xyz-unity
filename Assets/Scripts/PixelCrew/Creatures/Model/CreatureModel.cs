using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Creatures.Model
{
    [Serializable]
    public class CreatureModel
    {
        public int coins;
        public int hp = 100;
        public bool isArmed = false;
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