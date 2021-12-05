using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomizationWeaponUIHolder : MonoBehaviour
{
    public event Action<WeaponInfo> CustomizeWeaponPressed = delegate { };
    public event Action WeaponPressed = delegate { };
    [SerializeField] private Text weaponName;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button customizeWeaponBtn;
    private WeaponInfo currentWeaponInfo;
    
    private void Awake()
    {
        customizeWeaponBtn.onClick.AddListener(delegate {
            CustomizeWeaponPressed(currentWeaponInfo);
        });
        weaponBtn.onClick.AddListener(delegate
        {
            WeaponPressed();
        });
    }

    private void OnDestroy()
    {
        customizeWeaponBtn.onClick.RemoveAllListeners();
        weaponBtn.onClick.RemoveAllListeners();
    }

    public void setWeaponInfo(WeaponInfo weaponInfo)
    {
        this.currentWeaponInfo = weaponInfo;
        weaponName.text = weaponInfo.getID().ToString();
    }
}
