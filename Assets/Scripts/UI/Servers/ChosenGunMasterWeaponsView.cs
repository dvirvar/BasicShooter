using UnityEngine;
using System.Collections.Generic;

public class ChosenGunMasterWeaponsView : MonoBehaviour
{
    [SerializeField] private ChosenGunMasterWeaponsTableView chosenGunMasterWeaponsTableView;

    private void Awake()
    {
        for (int i = 1; i < 21; i++)
        {
            chosenGunMasterWeaponsTableView.addInfo(new ChosenGunMasterInfo(i));
        }
    }

    public bool setWeaponListDisplay(WeaponListDisplay weaponListDisplay)
    {
        foreach (var chosenDisplay in chosenGunMasterWeaponsTableView.getDisplays())
        {
            if (chosenDisplay.getWeaponListDisplay() == null)
            {
                chosenDisplay.setWeaponListDisplay(weaponListDisplay);
                return true;
            }
        }
        return false;
    }

    public void setWeaponListDisplays(List<WeaponListDisplay> weaponListDisplays) 
    {
        List<ChosenGunMasterWeaponDisplay> chosenGunMasterWeaponDisplays = chosenGunMasterWeaponsTableView.getDisplays();
        for (int i = 0; i < weaponListDisplays.Count; i++)
        {
            chosenGunMasterWeaponDisplays[i].setWeaponListDisplay(weaponListDisplays[i]);
        }
    }

    public List<WeaponListDisplay> getWeaponListDisplays()
    {
        List<WeaponListDisplay> weaponListDisplays = new List<WeaponListDisplay>();
        foreach (var item in chosenGunMasterWeaponsTableView.getDisplays())
        {
            WeaponListDisplay weaponListDisplay = item.getWeaponListDisplay();
            if (weaponListDisplay != null)
            {
                weaponListDisplays.Add(item.getWeaponListDisplay());
            }
        }
        return weaponListDisplays;
    }

    public void removeAllWeaponListDisplays()
    {
        chosenGunMasterWeaponsTableView.getDisplays().ForEach(chosenDisplay =>
        {
            if (chosenDisplay.getWeaponListDisplay() != null)
            {
                Destroy(chosenDisplay.getWeaponListDisplay().gameObject);
                chosenDisplay.clear();
            }
        });
    }
}
