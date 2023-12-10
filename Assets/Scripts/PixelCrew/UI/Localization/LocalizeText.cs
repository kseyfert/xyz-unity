using PixelCrew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : MonoBehaviour
    {
        [SerializeField] private string key;

        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            LocalizationManager.I.SubscribeAndInvoke(OnLocaleChanged);
        }

        private void OnLocaleChanged()
        {
            _text.text = LocalizationManager.I.Locale[key];
        }
    }
}