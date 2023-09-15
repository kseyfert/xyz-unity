using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField] private static int amount = 0;

        private static GameSession _gameSession;

        private void Start()
        {
            if (_gameSession != null) return;
            
            _gameSession = FindObjectOfType<GameSession>();
            LoadFromSession();
        }

        private void SaveToSession()
        {
            _gameSession.Data.coins = amount;
        }

        private void LoadFromSession()
        {
            amount = _gameSession.Data.coins;
        }

        public void AddAmount(int value)
        {
            value = Math.Max(0, value);
            amount += value;
            Debug.Log($"Money: {amount}");
            SaveToSession();
        }

        public void SubAmount(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(amount, value);
            amount -= value;
            Debug.Log($"Money: {amount}");
            SaveToSession();
        }

        public int GetAmount()
        {
            return amount;
        }
    }
}