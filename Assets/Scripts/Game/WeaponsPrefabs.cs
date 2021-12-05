using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsPrefabs", menuName = "Weapons/Prefabs")]
public class WeaponsPrefabs : ScriptableObject
{
    [SerializeField] private List<WeaponPrefabs> weaponsPrefabs;

    public WeaponStats getWeaponStats(WeaponID id)
    {
        return weaponsPrefabs.Find(wp => wp.weaponID == id).weaponStats;
    }

    public GameObject getSingleplayerPrefab(WeaponID id)
    {
        return weaponsPrefabs.Find(wp => wp.weaponID == id).singleplayerPrefab;
    }

    public GameObject getMultiplayerPrefab(WeaponID id)
    {
        return weaponsPrefabs.Find(wp => wp.weaponID == id).multiplayerPrefab;
    }

    public GameObject getCustomizablePrefab(WeaponID id)
    {
        return weaponsPrefabs.Find(wp => wp.weaponID == id).customizablePrefab;
    }
    public List<CustomizableWeapon> getCopyOfCustomizableWeapons()
    {
        List<CustomizableWeapon> newList = new List<CustomizableWeapon>(weaponsPrefabs.Count);
        for (int i = 0; i < weaponsPrefabs.Count; i++)
        {
            WeaponPrefabs weaponPrefabs = weaponsPrefabs[i];
            if (!User.currentUser().weaponsDictionary.ContainsKey(weaponPrefabs.weaponID)) {
                continue;
            }
            CustomizableWeapon customizableWeapon = new CustomizableWeapon(weaponPrefabs.weaponStats, User.currentUser().weaponsDictionary[weaponPrefabs.weaponID], weaponPrefabs.customizablePrefab);
            newList.Add(customizableWeapon.copy());
        }
        return newList;
    }

}

[System.Serializable]
public class WeaponPrefabs
{
    public WeaponID weaponID;
    public WeaponStats weaponStats;
    public GameObject singleplayerPrefab;
    public GameObject multiplayerPrefab;
    public GameObject customizablePrefab;
}

[System.Serializable]
public class CustomizableWeapon
{
    public WeaponStats weaponStats;
    public WeaponInfo weaponInfo;
    public GameObject prefab;

    public CustomizableWeapon(WeaponStats weaponStats, WeaponInfo weaponInfo, GameObject prefab)
    {
        this.weaponStats = weaponStats;
        this.weaponInfo = weaponInfo;
        this.prefab = prefab;
    }

    public CustomizableWeapon copy()
    {
        return new CustomizableWeapon(this.weaponStats, this.weaponInfo.copy(), this.prefab);
    }
}