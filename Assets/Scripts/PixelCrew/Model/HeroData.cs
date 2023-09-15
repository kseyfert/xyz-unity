using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class HeroData
    {
        public int coins = 0;
        public int hp = 100;
        public bool isArmed = false;
        public bool isDoubleJumpAllowed = false;
        public bool isInfiniteJumpAllowed = false;
        public List<LevelPosition> lastPositions;
        
        [Serializable]
        public struct LevelPosition
        {
            public string levelName;
            public Vector3 levelPosition;
        }
    }
}