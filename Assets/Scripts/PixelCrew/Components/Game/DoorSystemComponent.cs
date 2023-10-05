using UnityEngine;

namespace PixelCrew.Components.Game
{
    public class DoorSystemComponent : MonoBehaviour
    {
        private const int InitialValue = 7;
            
        [SerializeField] private SwitchComponent door1;
        [SerializeField] private SwitchComponent door2;
        [SerializeField] private SwitchComponent door3;

        private int _value = InitialValue;

        private void Awake()
        {
            UpdateDoors();
        }

        private void UpdateDoors()
        {
            int v = _value;

            door1.SetSwitched((v & 1) == 0);
            v >>= 1;
            
            door2.SetSwitched((v & 1) == 0);
            v >>= 1;
            
            door3.SetSwitched((v & 1) == 0);
        }

        public void Inc()
        {
            if (_value + 1 > InitialValue) return;
            
            _value++;
            UpdateDoors();
        }

        public void Dec()
        {
            if (_value - 2 < 0) return;
            
            _value -= 2;
            UpdateDoors();
        }

        public void Flush()
        {
            _value = InitialValue;
            UpdateDoors();
        }
    }
}