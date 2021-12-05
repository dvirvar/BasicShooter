using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent the object from the server
/// </summary>
[System.Serializable]
public abstract class BaseDTO
{
    
}

public class LoginDTO: BaseDTO
{
    public string id;
    public string token;
}

[System.Serializable]
public class PlayerLoadoutDTO: BaseDTO
{
    public int id;
    public WeaponID primaryweaponid;
    public WeaponID secondaryweaponid;
}