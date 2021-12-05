using System;

public class WeaponStatInfo : IEquatable<WeaponStatInfo>
{
    public WeaponStatType weaponStatType;
    public float value;

    public WeaponStatInfo(WeaponStatType weaponStatType, float value)
    {
        this.weaponStatType = weaponStatType;
        this.value = value;
    }

    public bool Equals(WeaponStatInfo other)
    {
        return value == other.value;
    }
}
