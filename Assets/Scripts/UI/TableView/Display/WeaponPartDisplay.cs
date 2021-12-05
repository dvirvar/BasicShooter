using UnityEngine.UI;
using System;

public class WeaponPartDisplay : Display<WeaponPartItem>
{
    public event Action<WeaponPartItem> OnClick = delegate { };
    private Image partImage;
    private Button button;

    private void Awake()
    {
        partImage = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            OnClick(info);
        });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    protected override void setView(WeaponPartItem info)
    {
        partImage.sprite = info.weaponPartPrefabHolder.sprite;
    }
    
}
