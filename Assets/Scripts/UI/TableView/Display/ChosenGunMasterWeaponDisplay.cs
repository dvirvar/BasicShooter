using System;
using UnityEngine;
using UnityEngine.UI;

public class ChosenGunMasterWeaponDisplay : Display<ChosenGunMasterInfo>
{
    [SerializeField] private Text numberText;
    [SerializeField] private GameObject weaponListDisplayContainer;
    private WeaponListDisplay weaponListDisplay;

    protected override void setView(ChosenGunMasterInfo info)
    {
        numberText.text = info.getName();
    }

    public WeaponListDisplay getWeaponListDisplay()
    {
        return weaponListDisplay;
    }

    public WeaponListDisplay setWeaponListDisplay(WeaponListDisplay weaponListDisplay)
    {
        WeaponListDisplay previous = this.weaponListDisplay;
        this.weaponListDisplay = weaponListDisplay;
        this.weaponListDisplay.setChosenGunMasterWeaponDisplay(this);
        this.weaponListDisplay.transform.SetParent(weaponListDisplayContainer.transform);
        #region workaround for items displayed not in position on first
        this.weaponListDisplay.rectTransform.anchorMin = new Vector2(0, 1);
        this.weaponListDisplay.rectTransform.anchorMax = new Vector2(0, 1);
        this.weaponListDisplay.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        this.weaponListDisplay.rectTransform.anchoredPosition = new Vector2(35,-35);
        #endregion
        this.weaponListDisplay.shrinkAnimation();//TODO: Removed it, And do fast shrink somehow
        return previous;
    }

    public void clear()
    {
        this.weaponListDisplay = null;
    }
}
