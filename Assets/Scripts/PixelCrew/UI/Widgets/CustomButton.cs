using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class CustomButton : Button
    {
        [SerializeField] private GameObject normal;
        [SerializeField] private GameObject pressed;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            
            if (normal != null) normal.SetActive(state != SelectionState.Pressed);
            if (pressed != null) pressed.SetActive(state == SelectionState.Pressed);
        }
    }
}