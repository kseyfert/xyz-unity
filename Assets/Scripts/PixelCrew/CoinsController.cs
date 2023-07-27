using UnityEngine;

namespace PixelCrew
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField] private static int amount = 0;

        public void AddAmount(int value)
        {
            amount += value;
            Debug.Log($"Money: {amount}");
        }
    }
}