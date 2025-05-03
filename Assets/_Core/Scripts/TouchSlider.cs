using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Core.Scripts
{
    public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityAction OnPointerDownEvent;
        public UnityAction OnPointerUpEvent;
        public UnityAction<float> OnPointerDragEvent;

        private Slider _uiSlider;

        private void Awake()
        {
            _uiSlider = GetComponent<Slider>();
            _uiSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke();
            OnPointerDragEvent?.Invoke(_uiSlider.value);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke();
            _uiSlider.value = 0;
        }

        private void OnSliderValueChanged(float value)
        {
            OnPointerDragEvent?.Invoke(value);
        }

        private void OnDestroy()
        {
            _uiSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}