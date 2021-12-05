using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SavedGunMasterCustomizationsDisplay : Display<GunMasterWeapons>, IPointerClickHandler
{
    public event Action<SavedGunMasterCustomizationsDisplay, PointerEventData.InputButton, int> OnClicks = delegate { };
    [SerializeField] private Text nameText;

    protected override void setView(GunMasterWeapons info)
    {
        this.nameText.text = info.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicks(this, eventData.button, eventData.clickCount);
    }
}
