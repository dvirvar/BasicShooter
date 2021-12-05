using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioRow : MonoBehaviour
{
    public event Action<float> onValueChanged = delegate { };
    [SerializeField] private Text nameText, percentageText;
    [SerializeField] private Slider slider;
    private float maxValue;

    private void Awake()
    {
        slider.onValueChanged.AddListener(value =>
        {
            if (value > maxValue)
            {
                setValue(maxValue);
                return;
            }
            onValueChanged(value);
            percentageText.text = $"{(int)(value * 100)}%";
        });
    }

    public void setView(string name, float value, float maxValue)
    {
        this.maxValue = maxValue;
        nameText.text = name;
        setValue(value);
    }

    public void setValue(float value)
    {
        percentageText.text = $"{(int)(value * 100)}%";
        slider.value = value;
    }

    public void setMaxValue(float maxValue)
    {
        this.maxValue = maxValue;
        if (slider.value > maxValue)
        {
            setValue(maxValue);
        }
    }

    public float getValue() => slider.value;


    private void OnDestroy()
    {
        slider.onValueChanged.RemoveAllListeners();   
    }
}
