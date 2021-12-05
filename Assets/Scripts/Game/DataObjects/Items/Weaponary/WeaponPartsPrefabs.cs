using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon parts prefabs", menuName = "Weapons/Weapon Parts")]
public class WeaponPartsPrefabs : ScriptableObject
{
    public List<ScopePrefabHolder> scopes;
    public GameObject getPrefab(ScopeID id)
    {
        return scopes.Find(gunMasterWeapon => gunMasterWeapon.id == id).prefab;
    }
}
