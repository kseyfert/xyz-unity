using PixelCrew.Model.Data.Properties.Persistent;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text value;

        private FloatPersistentProperty _model;

        private void Start()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            _model.OnChanged += OnModelValueChanged;
            OnModelValueChanged(_model.Value, _model.Value);
        }

        private void OnModelValueChanged(float oldValue, float newValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            value.text = textValue.ToString();
            // slider.normalizedValue = newValue;
            slider.SetValueWithoutNotify(newValue);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            _model.Value = sliderValue;
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            _model.OnChanged -= OnModelValueChanged;
        }
    }
}