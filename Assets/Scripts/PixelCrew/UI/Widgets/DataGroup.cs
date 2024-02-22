using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        protected readonly List<TItemType> createdItems = new List<TItemType>();
        
        private readonly TItemType _prefab;
        private readonly Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TDataType> data)
        {
            for (var i = createdItems.Count; i < data.Count; i++)
            {
                var item = Object.Instantiate(_prefab, _container);
                createdItems.Add(item);
            }

            for (var i = 0; i < data.Count; i++)
            {
                createdItems[i].SetData(data[i], i);
                createdItems[i].gameObject.SetActive(true);
            }

            for (var i = data.Count; i < createdItems.Count; i++)
            {
                createdItems[i].gameObject.SetActive(false);
            }
        }

        public virtual List<TItemType> GetActiveItems()
        {
            return createdItems.FindAll(item => item.gameObject.activeSelf);
        }
    }

    public interface IItemRenderer<in TDataType>
    {
        void SetData(TDataType data, int index);
    }
}