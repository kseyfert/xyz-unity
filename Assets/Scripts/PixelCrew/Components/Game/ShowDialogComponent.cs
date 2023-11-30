using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Hud.Dialogs;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Game
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode mode;

        [SerializeField] private DialogData bound;
        [SerializeField] private DialogDef external;

        [SerializeField] private UnityEvent onFinish;

        public void Show()
        {
            DialogBoxSingleton dialogBoxSingleton = SingletonMonoBehaviour.GetInstance<DialogBoxSingleton>();
            switch (mode)
            {
                case Mode.Bound:
                    dialogBoxSingleton.ShowDialog(bound, () => onFinish?.Invoke());
                    break;
                case Mode.External:
                    dialogBoxSingleton.ShowDialog(external.Data, () => onFinish?.Invoke());
                    break;
                default:
                    break;
            }
        }

        [Serializable]
        public enum Mode
        {
            Bound,
            External
        }
    }
}