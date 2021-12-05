using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class WeaponStatView : Display<WeaponStatInfo>
{

    [SerializeField] private Text titleText;
    [SerializeField] private Slider valueSlider;
    [SerializeField] private Image negativeFill;
    [SerializeField] private Image positiveFill;

    protected override void setView(WeaponStatInfo info)
    {
        titleText.text = info.weaponStatType.GetDescription();
        valueSlider.minValue = info.weaponStatType.minimumValue();
        valueSlider.maxValue = info.weaponStatType.maximumValue();
        setValue(info.value);
    }

    public void setValue(float value)
    {
        valueSlider.value = value;
        stopComparing();
    }

    public void compareValue(float value)
    {
        bool maxIsPositive = info.weaponStatType.moreIsPositive();
        if (value > valueSlider.value)
        {
            setFillAfterSliderFill(maxIsPositive ? positiveFill : negativeFill, value);
            positiveFill.gameObject.SetActive(maxIsPositive);
            negativeFill.gameObject.SetActive(!maxIsPositive);
        } else if (value < valueSlider.value)
        {
            setFillBeforeSliderFill(maxIsPositive ? negativeFill : positiveFill, value);
            negativeFill.gameObject.SetActive(maxIsPositive);
            positiveFill.gameObject.SetActive(!maxIsPositive);
        }
    }

    private void setFillAfterSliderFill(Image fill, float value)
    {
        fill.transform.SetAsFirstSibling();
        Vector2 fillMaxAnchor = valueSlider.fillRect.anchorMax;
        fill.rectTransform.anchorMin = new Vector2(fillMaxAnchor.x, 0);
        float fillMaxX = (value - valueSlider.minValue) / (valueSlider.maxValue - valueSlider.minValue);
        fill.rectTransform.anchorMax = new Vector2(fillMaxX, 1);
    }

    private void setFillBeforeSliderFill(Image fill, float value)
    {
        fill.transform.SetAsLastSibling();
        Vector2 fillMaxAnchor = valueSlider.fillRect.anchorMax;
        fill.rectTransform.anchorMax = new Vector2(fillMaxAnchor.x, 1);
        float fillMinX = (value - valueSlider.minValue) / (valueSlider.maxValue - valueSlider.minValue);
        fill.rectTransform.anchorMin = new Vector2(fillMinX, 0);
    }

    public void clearValue()
    {
        valueSlider.value = info.weaponStatType.minimumValue();
    }

    public void stopComparing()
    {
        negativeFill.gameObject.SetActive(false);
        positiveFill.gameObject.SetActive(false);
    }

}
