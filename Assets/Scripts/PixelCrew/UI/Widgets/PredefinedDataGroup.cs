using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class PredefinedDataGroup<TDataType, TItemType> : DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        public PredefinedDataGroup(Transform container) : base(null, container)
        {
            var items = container.GetComponentsInChildren<TItemType>();
            createdItems.AddRange(items);
        }

        public override void SetData(IList<TDataType> data)
        {
            if (data.Count > createdItems.Count) throw new Exception("Too many data items");
            base.SetData(data);
        }
    }
}