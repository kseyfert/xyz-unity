using System;
using UnityEngine;

namespace PixelCrew.Creatures.Controllers
{
    public class CoinsController : AController
    {
        [SerializeField] private Creature creature;
        [SerializeField] private int currentAmount = 0;

        private SessionController _sessionController;

        private void Start()
        {
            _sessionController = creature.SessionController;
            LoadFromSession();
        }

        private void SaveToSession()
        {
            if (_sessionController == null) return;
            
            _sessionController.GetModel().coins = currentAmount;
        }

        private void LoadFromSession()
        {
            if (_sessionController == null) return;

            currentAmount = _sessionController.GetModel().coins;
        }

        public int AddAmount(int value)
        {
            value = Math.Max(0, value);
            currentAmount += value;
            Debug.Log($"Money: {currentAmount}");
            
            SaveToSession();
            
            return value;
        }

        public int SubAmount(int value)
        {
            value = Math.Max(0, value);
            value = Math.Min(currentAmount, value);
            currentAmount -= value;
            Debug.Log($"Money: {currentAmount}");
            
            SaveToSession();

            return value;
        }

        public int ApplyAmount(int value)
        {
            return value > 0 ? AddAmount(value) : SubAmount(-value);
        }

        public int GetAmount()
        {
            return currentAmount;
        }

        protected override Creature GetCreature()
        {
            return creature;
        }
    }
}