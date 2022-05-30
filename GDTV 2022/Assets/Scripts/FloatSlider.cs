using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatSlider : MonoBehaviour
{
    public Slider slider;

    public Color low;

    public Color high;

    public void SetFloatValue(float setValue)
    {
        slider.value = setValue;
        slider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, slider.normalizedValue);
    }

    public void SetMaxFloatValue(float setValue)
    {
        slider.maxValue = setValue;
        slider.value = setValue;
    }
}
