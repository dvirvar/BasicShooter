using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class NetworkPlayer: MonoBehaviour
{
    [SerializeField] private NetworkDamageable networkDamageable;
    [SerializeField] private GameObject characterObject;
    [SerializeField] private LoadoutHolder loadoutHolder;
    [SerializeField] private PlayerGUI playerGUI;
    private PlayerInput playerInput; 
    private DonutColoring donutColoring;
    public PlayerObjects playerObjects { get; private set; }
    public string id { get; private set; }
    public int teamID { get; private set; }
    public PlayerState playerState { get; private set; }

    private void Awake()
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
        this.playerState = inGamePlayerInfo.playerState;
        networkDamageable.health = inGamePlayerInfo.health;
        setLoadout(inGamePlayerInfo.loadout, weaponsPrefabs);
    }

    public void semiInit(InGamePlayerInfo inGamePlayerInfo)
    {
        enablePlayer(false);
        this.id = inGamePlayerInfo.id;
        this.name = inGamePlayerInfo.name;
        donutColoring.setColors(inGamePlayerInfo.character.characterCustomization);
        this.teamID = inGamePlayerInfo.teamID;
    }

    public void enablePlayer(bool enable, bool isSelf)
    {
        gameObject.SetActive(enable);
        networkDamageable.enabled = isSelf && enable;
        playerInput.enabled = isSelf && enable;
        playerGUI.gameObject.SetActive(isSelf && enable);
        
    }

    public void enablePlayer(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void enablePlayerInput(bool enable)
    {
        playerInput.enabled = enable;
    }

    public void setLoadout(Loadout loadout, WeaponsPrefabs weaponsPrefabs)
    {
        Weapon primaryWeapon = null;
        Weapon secondaryWeapon = null;
        WeaponInfo primaryInfo = loadout.getPrimary();
        WeaponInfo secondaryInfo = loadout.getSecondary();
        if (primaryInfo != null)
        {
            GameObject primaryGO = Instantiate(weaponsPrefabs.getMultiplayerPrefab(primaryInfo.getID()));
            primaryWeapon = primaryGO.GetComponent<Weapon>();
            (primaryWeapon as NetworkWeapon).setNetworkIdentity(GetComponent<NetworkIdentity>());
            primaryWeapon.init(primaryInfo);
        }
        if (secondaryInfo != null)
        {
            GameObject secondaryGO = Instantiate(weaponsPrefabs.getMultiplayerPrefab(secondaryInfo.getID()));
            secondaryWeapon = secondaryGO.GetComponent<Weapon>();
            (secondaryWeapon as NetworkWeapon).setNetworkIdentity(GetComponent<NetworkIdentity>());
            secondaryWeapon.init(secondaryInfo);
        }
        loadoutHolder.init(primaryWeapon, secondaryWeapon);
    }

    public void setPing(int ping)
    {
        playerGUI.setPing(ping);
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

    public JSONObject getTransformToServer()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("id", id);
        JSONObject transform = JSONObject.obj;
        transform.AddField("rotation", JSONConvertUtil.vector3(this.transform.eulerAngles));
        transform.AddField("position", JSONConvertUtil.vector3(this.transform.position));
        data.AddField("transform", transform);
        return data;
    }

    public JSONObject getCharacterRotationToServer()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("id", id);
        data.AddField("localRotation", JSONConvertUtil.vector3(characterObject.transform.localEulerAngles));
        return data;
    }

    public LoadoutHolder getLoadoutHolder()
    {
        return loadoutHolder;
    }
}

public enum PlayerState
{
    Unspawned,Alive,Spectating,Dead
}

[Serializable]
[JsonConverter(typeof(PlayerTransformConverter))]
public class PlayerTransform
{
    public Vector3 position;
    public Vector3 rotation;
    [JsonConstructor]
    public PlayerTransform(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

class PlayerTransformConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(PlayerTransform);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var json = JObject.Load(reader);
        var positionJson = (JObject)json["position"];
        var position = new Vector3((float)positionJson["x"], (float)positionJson["y"], (float)positionJson["z"]);
        var rotationJson = (JObject)json["rotation"];
        var rotation = new Vector3((float)rotationJson["x"], (float)rotationJson["y"], (float)rotationJson["z"]);
        return new PlayerTransform(position, rotation);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var playerTransform = (PlayerTransform)value;
        writer.WriteStartObject();
        writer.WritePropertyName("position");

        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(playerTransform.position.x);
        writer.WritePropertyName("y");
        writer.WriteValue(playerTransform.position.y);
        writer.WritePropertyName("z");
        writer.WriteValue(playerTransform.position.z);
        writer.WriteEndObject();

        writer.WritePropertyName("rotation");
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(playerTransform.rotation.x);
        writer.WritePropertyName("y");
        writer.WriteValue(playerTransform.rotation.y);
        writer.WritePropertyName("z");
        writer.WriteValue(playerTransform.rotation.z);
        writer.WriteEndObject();

        writer.WriteEndObject();
        writer.Flush();
    }
}
