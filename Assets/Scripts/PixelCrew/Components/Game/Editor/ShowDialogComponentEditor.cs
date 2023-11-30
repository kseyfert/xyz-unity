using System;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.Game.Editor
{
    [CustomEditor(typeof(ShowDialogComponent))]
    public class ShowDialogComponentEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);

            var stringValue = _modeProperty.enumNames[_modeProperty.enumValueIndex];
            var mode = (ShowDialogComponent.Mode) Enum.Parse(typeof(ShowDialogComponent.Mode), stringValue);
            switch (mode)
            {
                case ShowDialogComponent.Mode.Bound:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bound"));
                    break;
                case ShowDialogComponent.Mode.External:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("external"));
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("onFinish"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}