using System;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.I.Items.ItemsForEditor;
            var ids = Array.ConvertAll(defs, item => item.Id);

            var index = Array.IndexOf(ids, property.stringValue);
            if (index < 0) index = 0;
            
            index = EditorGUI.Popup(position, property.displayName, index, ids);
            property.stringValue = ids[index];
        }
        
    }
}