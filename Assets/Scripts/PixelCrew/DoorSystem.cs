using System;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{
    public class DoorSystem : MonoBehaviour
    {
        private const int InitialValue = 7;
            
        [SerializeField] private SwitchComponent door1;
        [SerializeField] private SwitchComponent door2;
        [SerializeField] private SwitchComponent door3;

        private int value = InitialValue;

        private void Awake()
        {
            UpdateDoors();
        }

        private void UpdateDoors()
        {
            int v = value;

            door1.SetSwitched((v & 1) == 0);
            v >>= 1;
            
            door2.SetSwitched((v & 1) == 0);
            v >>= 1;
            
            door3.SetSwitched((v & 1) == 0);
        }

        public void Inc()
        {
            if (value + 1 > InitialValue) return;
            
            value++;
            UpdateDoors();
        }

        public void Dec()
        {
            if (value - 2 < 0) return;
            
            value -= 2;
            UpdateDoors();
        }

        public void Flush()
        {
            value = InitialValue;
            UpdateDoors();
        }
    }
}