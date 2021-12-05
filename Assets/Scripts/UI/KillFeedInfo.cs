using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillFeedInfo : IEquatable<KillFeedInfo>
{
    public string killer;
    public string victim;
    public string weapon;

    public bool Equals(KillFeedInfo other)
    {
        return (killer == other.killer && victim == other.victim && weapon == other.weapon);
    }
}
