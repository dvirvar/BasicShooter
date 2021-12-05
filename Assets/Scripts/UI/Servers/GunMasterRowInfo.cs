using System.Collections;
using System;
using UnityEngine;

public class GunMasterRowInfo : IEquatable<GunMasterRowInfo>
{
    public string weaponName;
    public int level;
    public int numOfPlayers = 0;
    public bool isSelfHere;

    public bool Equals(GunMasterRowInfo other)
    {
        return level == other.level;
    }
}
