using UnityEngine;
/// <summary>
/// Stores the info of the gun master
/// </summary>
[CreateAssetMenu(fileName = "Gun Master Info", menuName = "Game Modes/Gun Master")]
public class GunMasterInfo : GameModeInfo
{
    
    public int levels => weapons.Length;
    public int toLevelUp;
    public GameObject[] weapons;

    public bool hasPlayerWon(int weaponLevel)
    {
        return weaponLevel >= levels;
    }

}