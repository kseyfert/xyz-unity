using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Utils
{
    public class SingletonMonoBehaviour : MonoBehaviour
    {
        private static readonly List<Type> InstancesTypes = new List<Type>();
        private static readonly List<SingletonMonoBehaviour> Instances = new List<SingletonMonoBehaviour>();

        public static TType GetInstance<TType>() where TType : SingletonMonoBehaviour
        {
            return LoadAndGet<TType>();
        }

        public static void Load<TType>() where TType : SingletonMonoBehaviour
        {
            LoadAndGet<TType>();
        }

        private static TType LoadAndGet<TType>() where TType : SingletonMonoBehaviour
        {
            var foundIndex = InstancesTypes.FindIndex(typeItem => typeItem == typeof(TType));
            var foundInstance = foundIndex >= 0 ? Instances[foundIndex] : null;

            if (foundInstance != null && foundInstance.gameObject.activeSelf) return (TType)foundInstance;

            if (foundInstance != null && !foundInstance.gameObject.activeSelf)
            {
                InstancesTypes.RemoveAt(foundIndex);
                Instances.RemoveAt(foundIndex);
            }

            var allObjects = new List<TType>(FindObjectsOfType<TType>())
                .FindAll(item => item.gameObject.activeSelf);
            
            var newInstance = allObjects.Find(obj => obj.singletonMain);

            if (newInstance == null) newInstance = allObjects.First();
            if (newInstance == null) return null;
            
            allObjects
                .FindAll(obj => obj != newInstance)
                .ForEach(obj => Destroy(obj.gameObject));
            
            InstancesTypes.Add(typeof(TType));
            Instances.Add(newInstance);

            return (TType)Instances.Last();
        }

        [SerializeField] private bool singletonMain = false;
    }
}