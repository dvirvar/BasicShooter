using System.Collections.Generic;
using System;

public class User
{
    private static User current;

    public static User currentUser()
    {
        if (current == null)
        {
            current = new User();
        }
        return current;
    }

    public string id;
    public string token;
    public string name;
    public PlayerCustomization playerCustomization;
    public Dictionary<WeaponID, WeaponInfo> weaponsDictionary = new Dictionary<WeaponID, WeaponInfo>();
    public PlayerLoadouts playerLoadouts;

    public void setWeapons(Dictionary<WeaponID,WeaponInfo> weapons)
    {
        weaponsDictionary = weapons;
    }

    public void setWeapons(WeaponInfo[] weapons)
    {
        weaponsDictionary.Clear();
        for (int i = 0; i < weapons.Length; i++)
        {
            var weapon = weapons[i];
            weaponsDictionary[weapon.getID()] = weapon;
        }
    }

    public void reset()
    {
        id = null;
        token = null;
        name = null;
        playerCustomization = null;
        weaponsDictionary.Clear();
        playerLoadouts = null;
    }
}
