using System;
using UnityEngine;

namespace PixelCrew
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField] private static int amount = 0;

        public void AddAmount(int value)
        {
            value = Math.Max(0, value);
            amount += value;
            Debug.Log($"Money: {amount}");
        }

        public void SubAmount(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(amount, value);
            amount -= value;
            Debug.Log($"Money: {amount}");
        }

        public int GetAmount()
        {
            return amount;
        }
    }
}