using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public struct ServerGunMasterInfo
{
    public List<WeaponID> weaponsIDs;
    public int toLevelUp;
    public Dictionary<string, int> points;

    public override string ToString()
    {
        return $"weapons {string.Join(",", weaponsIDs)}, levelUp {toLevelUp}, points: {points}";
    }
}
