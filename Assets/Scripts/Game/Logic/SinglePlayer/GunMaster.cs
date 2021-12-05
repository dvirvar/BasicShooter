using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the logic of gun master
/// </summary>
public class GunMaster: GameMode
{
    public static event Action<string> playerHasWon = delegate { };
    private Dictionary<string, int> playersPoints = new Dictionary<string, int>();
    protected GunMasterInfo gunMasterInfo;

    protected void Start()
    {
        
    }
    
    protected void Update()
    {

    }

    //Returns weapon
    public GameObject addPoint(string id)
    {
        if (playersPoints.ContainsKey(id))
        {
            playersPoints[id] += 1;
        }
        else
        {
            playersPoints[id] = 1;
        }
        int weaponLevel = playersPoints[id] / gunMasterInfo.toLevelUp;
        if (gunMasterInfo.hasPlayerWon(weaponLevel))
        {
            playerHasWon(id);
            return gunMasterInfo.weapons[gunMasterInfo.levels - 1];
        }
        return gunMasterInfo.weapons[weaponLevel];
    }

    //Returns weapon
    public GameObject removePoint(string id)
    {
        if (playersPoints.ContainsKey(id))
        {
            if (playersPoints[id] != 0)
            {
                playersPoints[id] -= 1;
            }
        }
        else
        {
            playersPoints[id] = 0;//Null Pointer Exception
        }
        int weaponLevel = playersPoints[id] / gunMasterInfo.toLevelUp;
        return gunMasterInfo.weapons[weaponLevel];
    }

    protected void OnDestroy()
    {
        
    }
    
}