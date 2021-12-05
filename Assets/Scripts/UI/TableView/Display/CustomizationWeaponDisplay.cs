using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class CustomizationWeaponDisplay : Display<WeaponListItem>, IPointerEnterHandler, IPointerExitHandler
{

    public event Action<CustomizableWeapon> OnClick = delegate { };
    public event Action<bool, CustomizableWeapon> OnHover = delegate { };
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text weaponText;
    [SerializeField] private Image chosenImage;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            OnClick(info.customizableWeapon);
        });
    }

    public void setChosen(bool chosen)
    {
        chosenImage.gameObject.SetActive(chosen);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    protected override void setView(WeaponListItem info)
    {
        weaponText.text = info.name;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover(true, info.customizableWeapon);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover(false, info.customizableWeapon);
    }
}
