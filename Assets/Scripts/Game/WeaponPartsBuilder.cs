using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponPartsBuilder : MonoBehaviour
{
    public event Action<WeaponPartType, Transform> onPartLocationPresssed = delegate { };
    [SerializeField] private WeaponPartsPrefabs weaponPartsPrefabs;
    private Dictionary<WeaponPartType, WeaponPartHolder> weaponPartHolders;
    private WeaponInfo weaponInfo;

    private void Awake()
    {
        weaponPartHolders = new Dictionary<WeaponPartType, WeaponPartHolder>();
        WeaponPartHolder[] wphs = GetComponentsInChildren<WeaponPartHolder>();
        foreach (var wph in wphs)
        {
            weaponPartHolders[wph.getWeaponPartType()] = wph;
            wph.onCustomizationButtonPressed += Wph_onCustomizationButtonPressed;
        }
    }

    private void OnDestroy()
    {
        foreach (var wph in weaponPartHolders)
        {
            wph.Value.onCustomizationButtonPressed -= Wph_onCustomizationButtonPressed;
        }
    }

    private void Wph_onCustomizationButtonPressed(WeaponPartType wpt, Transform transform)
    {
        onPartLocationPresssed(wpt, transform);
    }

    public void hidePartsLocations()
    {
        foreach (var wph in weaponPartHolders)
        {
            wph.Value.hideWeaponPartLocation();
        }
    }

    public void showPartsLocations()
    {
        foreach (var wph in weaponPartHolders)
        {
            wph.Value.showWeaponPartLocation();
        }
    }

    public void setWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        setScope(weaponInfo.scopeID);
    }

    public void setScope(ScopeID id)
    {
        weaponPartHolders.TryGetValue(WeaponPartType.scope,out WeaponPartHolder scopeHolder);
        if (scopeHolder != null)
        {
            scopeHolder.clear();
            if (id != ScopeID.none)
            {
                scopeHolder.instantiatePart(weaponPartsPrefabs.getPrefab(id));
            }

            if (this.weaponInfo.scopeID != id)
            {
                this.weaponInfo.scopeID = id;
            }
        } else if (id != ScopeID.none)
        {
            Debug.LogError($"{WeaponPartType.scope} doesnt exist in this weapon {weaponInfo.getID()}");
        }
    }
}
