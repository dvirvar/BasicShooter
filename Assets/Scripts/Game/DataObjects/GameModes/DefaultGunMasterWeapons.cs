using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "Default Gun Master Weapons", menuName = "Game Modes/Default Gun Master Weapons")]
public class DefaultGunMasterWeapons: ScriptableObject
{
    public List<GunMasterWeapons> gunMasterWeapons;
    public GunMasterWeapons getGunMasterWeapons(string name)
    {
        return gunMasterWeapons.Find(gunMasterWeapon => gunMasterWeapon.name == name);
    }
    public WeaponInfo[] getWeapons(string name)
    {
        return gunMasterWeapons.Find(gunMasterWeapon => gunMasterWeapon.name == name).weapons;
    }
    public bool contains(string name)
    {
        return gunMasterWeapons.Find(gunMasterWeapon => gunMasterWeapon.name.ToLower() == name.ToLower()) != null;
    }
}

[Serializable]
public class GunMasterWeapons: IEquatable<GunMasterWeapons>
{
    public string name;
    public WeaponInfo[] weapons;

    public bool Equals(GunMasterWeapons other)
    {
        return name == other.name;
    }

    public JSONObject toJSONObject()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("name", name);
        data.AddField("weapons", JSONObject.Create(JsonConvert.SerializeObject(weapons)));
        return data;
    }
}