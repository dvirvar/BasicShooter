using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Loadout
{
    [JsonProperty] private int id;
    [JsonProperty] [SerializeField] private WeaponInfo primary;
    [JsonProperty] [SerializeField] private WeaponInfo secondary;

    public Loadout(int id, WeaponInfo primary, WeaponInfo secondary)
    {
        setPrimary(primary);
        setSecondary(secondary);
        this.id = id;
    }

    public WeaponInfo getPrimary()
    {
        return primary;
    }

    public WeaponInfo getSecondary()
    {
        return secondary;
    }

    public void setPrimary(WeaponInfo primary)
    {
        if (primary != null && primary.getWeaponType() != WeaponType.Primary)
        {
            throw new System.DataMisalignedException("Tried to enter weapon that is not primary to primary slot");
        }
        this.primary = primary;
    }

    public void setSecondary(WeaponInfo secondary)
    {
        if (secondary != null && secondary.getWeaponType() != WeaponType.Secondary)
        {
            throw new System.DataMisalignedException("Tried to enter weapon that is not secondary to secondary slot");
        }
        this.secondary = secondary;
    }

    public void setWeaponByType(WeaponInfo weapon)
    {
        if (weapon.getWeaponType() == WeaponType.Primary)
        {
            setPrimary(weapon);
        } else if (weapon.getWeaponType() == WeaponType.Secondary)
        {
            setSecondary(weapon);
        }
    }

    public bool hasWeapon(WeaponInfo weapon)
    {
        return (weapon.getWeaponType() == WeaponType.Primary && primary.getID() == weapon.getID()) || (weapon.getWeaponType() == WeaponType.Secondary && secondary.getID() == weapon.getID());
    }

    public override string ToString()
    {
        return $"id: {id}, primary: {primary}, secondary: {secondary}";
    }
}
