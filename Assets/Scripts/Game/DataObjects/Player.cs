using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    [SerializeField] private GameObject characterObject;
    [SerializeField] private LoadoutHolder loadoutHolder;
    [SerializeField] private PlayerGUI playerGUI;
    private PlayerInput playerInput;
    private DonutColoring donutColoring;
    public PlayerObjects playerObjects { get; private set; }
    public string id { get; private set; }
    public int teamID { get; private set; }
    public PlayerState playerState { get; private set; }
    
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        donutColoring = characterObject.GetComponent<DonutColoring>();
        playerObjects = GetComponent<PlayerObjects>();
    }

    public void init(InGamePlayerInfo inGamePlayerInfo, WeaponsPrefabs weaponsPrefabs)
    {
        semiInit(inGamePlayerInfo);
        setTransform(inGamePlayerInfo.transform);
        setCharacterRotation(inGamePlayerInfo.character.rotation);
        playerState = inGamePlayerInfo.playerState;
        damageable.health = inGamePlayerInfo.health;
        setLoadout(inGamePlayerInfo.loadout, weaponsPrefabs);
    }

    private void semiInit(InGamePlayerInfo inGamePlayerInfo)
    {
        this.id = inGamePlayerInfo.id;
        this.name = inGamePlayerInfo.name;
        donutColoring.setColors(inGamePlayerInfo.character.characterCustomization);
        this.teamID = inGamePlayerInfo.teamID;
    }

    public void enablePlayer(bool enable, bool isSelf)
    {
        gameObject.SetActive(enable);
        if (isSelf)
        {
            playerInput.enabled = enable;
            playerGUI.gameObject.SetActive(enable);
        }
    }

    public void enablePlayer(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void setLoadout(Loadout loadout, WeaponsPrefabs weaponsPrefabs)
    {
        Weapon primaryWeapon = null;
        Weapon secondaryWeapon = null;
        WeaponInfo primaryInfo = loadout.getPrimary();
        WeaponInfo secondaryInfo = loadout.getSecondary();
        if (primaryInfo != null)
        {
            GameObject primaryGO = Instantiate(weaponsPrefabs.getSingleplayerPrefab(primaryInfo.getID()));
            primaryWeapon = primaryGO.GetComponent<Weapon>();
            primaryWeapon.init(primaryInfo);
        }
        if (secondaryInfo != null)
        {
            GameObject secondaryGO = Instantiate(weaponsPrefabs.getSingleplayerPrefab(secondaryInfo.getID()));
            secondaryWeapon = secondaryGO.GetComponent<Weapon>();
            secondaryWeapon.init(secondaryInfo);
        }
        loadoutHolder.init(primaryWeapon, secondaryWeapon);
    }

    public void setTransform(PlayerTransform playerTransform)
    {
        transform.position = playerTransform.position;
        transform.eulerAngles = playerTransform.rotation;
    }

    public void setCharacterRotation(Vector3 characterRotation)
    {
        characterObject.transform.localEulerAngles = characterRotation;
    }
}
