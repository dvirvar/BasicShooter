using System;
using ExtensionMethods;

public class WeaponPartItem : IEquatable<WeaponPartItem>
{
    public readonly WeaponPartPrefabHolder<int> weaponPartPrefabHolder;
    public readonly string searchKeyWord;

    public WeaponPartItem(ScopePrefabHolder sph)
    {
        this.weaponPartPrefabHolder = sph.convertToInt();
        searchKeyWord = sph.id.GetDescription();
    }

    public bool Equals(WeaponPartItem other)
    { 
        return weaponPartPrefabHolder.id.Equals(other.weaponPartPrefabHolder.id);
    }

}
