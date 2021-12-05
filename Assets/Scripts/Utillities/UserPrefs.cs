using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class UserPrefs
{
    private static readonly string filter = "filter"; 
    public static void setFilter(FilterInfo filterInfo)
    {
        string key = $"{User.currentUser().id}{filter}";
        PlayerPrefs.SetString(key, JsonUtility.ToJson(filterInfo));
    }

    public static FilterInfo getFilter()
    {
        string key = $"{User.currentUser().id}{filter}";
        return JsonUtility.FromJson<FilterInfo>(PlayerPrefs.GetString(key));
    }

    private static readonly string gunMasterWeapons = "gunMasterWeapons";
    public static void setGunMasterWeapons(List<GunMasterWeapons> weapons)
    {
        string key = $"{User.currentUser().id}{gunMasterWeapons}";
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(weapons));
    }

    public static void addGunMasterWeapons(GunMasterWeapons weapons)
    {
        List<GunMasterWeapons> weaponsList = getGunMasterWeapons();
        int indexOfExistingGunMasterWeapons = weaponsList.FindIndex(gmw => gmw.Equals(weapons));
        if (indexOfExistingGunMasterWeapons > -1)
        {
            weaponsList[indexOfExistingGunMasterWeapons] = weapons;
        } else
        {
            weaponsList.Add(weapons);
        }
        setGunMasterWeapons(weaponsList);
    }

    public static void deleteGunMasterWeapons(GunMasterWeapons weapons)
    {
        List<GunMasterWeapons> currentGunMasterWeapons = getGunMasterWeapons();
        currentGunMasterWeapons.Remove(weapons);
        setGunMasterWeapons(currentGunMasterWeapons);
    }

    public static List<GunMasterWeapons> getGunMasterWeapons()
    {
        string key = $"{User.currentUser().id}{gunMasterWeapons}";
        return JsonConvert.DeserializeObject<List<GunMasterWeapons>>(PlayerPrefs.GetString(key)) ?? new List<GunMasterWeapons>();
    }

    private static readonly string gunMasterWeaponsLastSelected = "gunMasterWeaponsLastSelected";
    public static void setGunMasterWeaponsLastSelected(string name)
    {
        string key = $"{User.currentUser().id}{gunMasterWeaponsLastSelected}";
        PlayerPrefs.SetString(key, name);
    }

    public static string getGunMasterWeaponsLastSelected()
    {
        string key = $"{User.currentUser().id}{gunMasterWeaponsLastSelected}";
        return PlayerPrefs.GetString(key);
    }

    public static void setVolumeGroupValue(AudioHandler.VolumeGroup volumeGroup, float value)
    {
        string key = $"{User.currentUser().id}{volumeGroup}";
        PlayerPrefs.SetFloat(key, value);
    }

    public static float getVolumeGroupValue(AudioHandler.VolumeGroup volumeGroup)
    {
        string key = $"{User.currentUser().id}{volumeGroup}";
        return PlayerPrefs.GetFloat(key, 0);
    }
}
