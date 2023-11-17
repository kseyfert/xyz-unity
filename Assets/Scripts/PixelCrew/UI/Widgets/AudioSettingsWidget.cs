using PixelCrew.Model.Data.Properties.Persistent;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text value;

        private FloatPersistentProperty _model;

        private CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(slider.onValueChanged.Subscribe(OnSliderValueChanged));
        }

        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            _trash.Retain(_model.Subscribe(OnModelValueChanged));
            OnModelValueChanged(_model.Value, _model.Value);
        }

        private void OnModelValueChanged(float oldValue, float newValue)
        {
            var textValue = Mathf.Round(newValue * 100);
            value.text = textValue.ToString();
            slider.SetValueWithoutNotify(newValue);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            _model.Value = sliderValue;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}