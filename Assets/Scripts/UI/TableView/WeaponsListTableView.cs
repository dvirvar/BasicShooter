using System.Collections.Generic;

public class WeaponsListTableView : TableView<WeaponListItem,WeaponListDisplay>
{
    public void setWeaponListDisplay(WeaponListDisplay weaponListDisplay, int index)
    {
        if (weaponListDisplay.chosenState)
        {
            weaponListDisplay.clearChosenGunMasterWeaponDisplay();
        }
        weaponListDisplay.growAnimation();
        weaponListDisplay.transform.SetParent(contentTransform);
        weaponListDisplay.transform.SetSiblingIndex(index);
        infosDisplay.Insert(index, weaponListDisplay);
    }

    public void removeWeaponListDisplay(WeaponListDisplay weaponListDisplay)
    {
        infosDisplay.Remove(weaponListDisplay);
    }

    public void showAll()
    {
        infosDisplay.ForEach(display => display.gameObject.SetActive(true));
    }

    public void showAll(List<WeaponListItem> weaponListItems)
    {
        hideAll();
        foreach (var item in weaponListItems)
        {
            int indexOfDisplay = infosDisplay.FindIndex(wld => wld.info.Equals(item));
            if (indexOfDisplay > -1)
            {
                infosDisplay[indexOfDisplay].gameObject.SetActive(true);
            }
        }
    }

    public void hideAll()
    {
        infosDisplay.ForEach(display => display.gameObject.SetActive(false));
    }
}
