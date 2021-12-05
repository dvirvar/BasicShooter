using System;
using UnityEngine;

public class WeaponListItem : IEquatable<WeaponListItem>
{
    public readonly string name;
    public WeaponInfo weaponInfo;
    public readonly CustomizableWeapon customizableWeapon;
    public WeaponStats weaponStats => customizableWeapon.weaponStats;
    public GameObject weaponPrefab => customizableWeapon.prefab;

    public WeaponListItem(CustomizableWeapon customizableWeapon)
    {
        this.customizableWeapon = customizableWeapon;
        this.weaponInfo = customizableWeapon.weaponInfo;
        this.name = weaponStats.id.ToString();
    }

    public void setWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    public bool Equals(WeaponListItem other)
    {
        return weaponInfo.Equals(other.weaponInfo);
    }
}
