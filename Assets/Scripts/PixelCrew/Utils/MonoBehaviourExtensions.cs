using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class MonoBehaviourExtensions
    {
        public static void SetTimeout(this MonoBehaviour monoBehaviour, Action action, float timeout)
        {
            monoBehaviour.StartCoroutine(TimeoutCoroutine(action, timeout));
        }
        
        private static IEnumerator TimeoutCoroutine(Action action, float timeout)
        {
            yield return new WaitForSeconds(timeout);
            action?.Invoke();
        } 
    }
}