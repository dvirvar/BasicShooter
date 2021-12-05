using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSciprt : MonoBehaviour
{
    [SerializeField] private WeaponsPrefabs weaponsPrefabs;
    [SerializeField] private Player player;
    [SerializeField] private Player enemy;

    void Start()
    {
        var info = new InGamePlayerInfo();
        info.id = "Player";
        info.health = 100;
        info.name = "Player";
        info.transform = new PlayerTransform(Vector3.up, Vector3.zero);
        info.character = new Character(Vector3.zero, new PlayerCustomization("#4287f5", "#42f57b"));
        info.teamID = 0;
        info.playerState = PlayerState.Alive;
        info.loadout = new Loadout(0, new WeaponInfo(WeaponID.AK47, WeaponType.Primary, ScopeID.none), new WeaponInfo(WeaponID.M1911, WeaponType.Secondary, ScopeID.none));
        player.init(info, weaponsPrefabs);
        player.enablePlayer(true, true);

        var info2 = new InGamePlayerInfo();
        info2.id = "Enemy";
        info2.health = 100;
        info2.name = "Enemy";
        info2.transform = new PlayerTransform(new Vector3(4,1,2), Vector3.zero);
        info2.character = new Character(Vector3.zero, new PlayerCustomization("#2287f5", "#12f57b"));
        info2.teamID = 1;
        info2.playerState = PlayerState.Alive;
        info2.loadout = new Loadout(0, new WeaponInfo(WeaponID.M16, WeaponType.Primary, ScopeID.none), new WeaponInfo(WeaponID.ACP, WeaponType.Secondary, ScopeID.none));
        enemy.init(info2, weaponsPrefabs);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Cursor.visible = Cursor.lockState == CursorLockMode.None;
        }
    }
}