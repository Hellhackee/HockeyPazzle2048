using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction OnPointerUpHandler;
    public UnityAction OnPointerDownHandler;
    public UnityAction<float> OnSliderDragHandler;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnSliderDrag);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownHandler?.Invoke();
        OnSliderDragHandler?.Invoke(_slider.value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpHandler?.Invoke();

        _slider.value = 0f;
    }

    private void OnSliderDrag(float value)
    {
        OnSliderDragHandler?.Invoke(value);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderDrag);
    }

    public float GetSliderValue()
    {
        return _slider.value;
    }
}
