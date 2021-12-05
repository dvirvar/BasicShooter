using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

public class WeaponStatsView : MonoBehaviour
{

    [SerializeField] private WeaponStatsTableView weaponStatsTableView;
    private Dictionary<WeaponStatType, WeaponStatView> weaponsStatViews;
    private WeaponStats currentWeaponStats;

    private void Awake()
    {
        weaponsStatViews = new Dictionary<WeaponStatType, WeaponStatView>();
    }

    void Start()
    {
        var weaponStatTypes = (WeaponStatType[])Enum.GetValues(typeof(WeaponStatType));
        for (int i = 0; i < weaponStatTypes.Length; i++)
        {
            WeaponStatType weaponStatType = weaponStatTypes[i];
            weaponStatsTableView.addInfo(new WeaponStatInfo(weaponStatType, weaponStatType.minimumValue()));
            weaponsStatViews[weaponStatType] = weaponStatsTableView.getDisplay(i);
        }
    }

    public void setValues(WeaponStats weaponStats)
    {
        this.currentWeaponStats = weaponStats;
        foreach (var weaponsStatView in weaponsStatViews)
        {
            weaponsStatView.Value.setValue(weaponsStatView.Key switch {
                WeaponStatType.FirePower => weaponStats.firePower,
                WeaponStatType.FireRate => weaponStats.fireRate,
                WeaponStatType.ReloadSpeed => weaponStats.reloadSpeed,
                WeaponStatType.Recoil => weaponStats.recoil,
                WeaponStatType.Damage => weaponStats.damage,
                _ => throw new NotImplementedException("WeaponStatType not implemented")
            });
        }
    }

    public void clear()
    {
        currentWeaponStats = null;
        foreach (var weaponsStatView in weaponsStatViews)
        {
            weaponsStatView.Value.clearValue();
        }
    }

    public void compareValues(WeaponStats weaponStats)
    {
        if (currentWeaponStats == null)
        {
            return;
        }
        foreach (var weaponsStatView in weaponsStatViews)
        {
            weaponsStatView.Value.compareValue(weaponsStatView.Key switch
            {
                WeaponStatType.FirePower => weaponStats.firePower,
                WeaponStatType.FireRate => weaponStats.fireRate,
                WeaponStatType.ReloadSpeed => weaponStats.reloadSpeed,
                WeaponStatType.Recoil => weaponStats.recoil,
                WeaponStatType.Damage => weaponStats.damage,
                _ => throw new NotImplementedException("WeaponStatType not implemented")
            });
        }
    }

    public void stopComparingValues()
    {
        foreach (var weaponsStatView in weaponsStatViews)
        {
            weaponsStatView.Value.stopComparing();   
        }
    }

}
