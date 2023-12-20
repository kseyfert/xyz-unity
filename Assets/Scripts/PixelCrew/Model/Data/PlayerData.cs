using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties.Observable;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        public static readonly string Coins = "Coins";
        public static readonly string Weapons = "Weapons";
        public static readonly string DoubleJumper = "DoubleJumper";
        public static readonly string InfiniteJumper = "InfiniteJumper";
        public static readonly string Keys = "Keys";

        public InventoryData inventory = new InventoryData();
        
        public IntObservableProperty hp = new IntObservableProperty();

        public void Update(PlayerData other)
        {
            inventory.Update(other.inventory);
            hp.Value = other.hp.Value;
        }

        public void UpdateFromJson(string json)
        {
            var other = JsonUtility.FromJson<PlayerData>(json);
            Update(other);
        }
    }
}