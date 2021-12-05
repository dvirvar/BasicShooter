using UnityEngine;
using System;

/// <summary>
/// Represent a weapon part
/// </summary>
/// <typeparam name="T">The id</typeparam>
public class WeaponPartPrefabHolder<T>
{
    public T id;
    public GameObject prefab;
    public Sprite sprite;

    public virtual WeaponPartPrefabHolder<int> convertToInt()
    {
        throw new NotImplementedException("convertToInt not implemented");
    }
}

[System.Serializable]
public class ScopePrefabHolder: WeaponPartPrefabHolder<ScopeID>
{
    public override WeaponPartPrefabHolder<int> convertToInt()
    {
        WeaponPartPrefabHolder<int> s = new WeaponPartPrefabHolder<int>();
        s.id = (int)id;
        s.prefab = prefab;
        s.sprite = sprite;
        return s;
    }
}