using UnityEngine;
using System.ComponentModel;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Weapons/Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public WeaponID id;
    public int bullets;
    public int magazineCapacity;
    public int firePower;
    public float fireRate;
    public float reloadSpeed;
    public float recoil;
    public int damage;
    public WeaponModes[] weaponModes;

    public PoolObjectID bulletID;
    public AudioClip fireSound;
    public AudioClip reloadSound;

}

public enum WeaponStatType
{
    [Description("Fire Power")]
    FirePower,
    [Description("Fire Rate")]
    FireRate,
    [Description("Reload Speed")]
    ReloadSpeed,
    [Description("Recoil")]
    Recoil,
    [Description("Damage")]
    Damage
}