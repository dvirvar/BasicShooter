using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponsListView : MonoBehaviour
{

    [SerializeField] private InputField searchWeaponIF;
    [SerializeField] private WeaponsListTableView weaponsListTV;
    private List<WeaponListItem> weaponListItems;

    private void Awake()
    {
        weaponListItems = new List<WeaponListItem>();

        searchWeaponIF.onValueChanged.AddListener(delegate
        {
            searchWeapon();
        });
    }

    public void setWeaponListItems(List<WeaponListItem> weaponListItems)
    {
        this.weaponListItems = weaponListItems;
        weaponsListTV.setInfos(weaponListItems);
    }

    public void setWeaponListDisplay(WeaponListDisplay weaponListDisplay, int index)
    {
        weaponListItems.Insert(index, weaponListDisplay.info);
        weaponsListTV.setWeaponListDisplay(weaponListDisplay, index);
    }

    public void removeWeaponListDisplay(WeaponListDisplay weaponListDisplay)
    {
        weaponListItems.Remove(weaponListDisplay.info);
        weaponsListTV.removeWeaponListDisplay(weaponListDisplay);
    }

    public List<WeaponListDisplay> getWeaponListDisplays()
    {
        return weaponsListTV.getDisplays();
    }

    public void removeAllWeaponListDisplays()
    {
        weaponsListTV.removeAll();
        weaponListItems.Clear();
    }

    private void searchWeapon()
    {
        
        if (searchWeaponIF.text != "")
        {
            weaponsListTV.showAll(weaponListItems.FindAll(weapon =>
            {
                return weapon.name.ToString().ToLower().Contains(searchWeaponIF.text.ToLower());
            }));
        }
        else
        {
            weaponsListTV.showAll();
        }
    }

    private void OnDestroy()
    {
        searchWeaponIF.onValueChanged.RemoveAllListeners();
    }

}
