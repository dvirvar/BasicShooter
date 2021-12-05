using System.Collections.Generic;
using UnityEngine;
using System;
using ExtensionMethods;

/// <summary>
/// Managing all objects on server
/// </summary>
public class NetworkObjectsManager : MonoBehaviour
{
    public static event Action<NetworkPlayer> OnSelfSpawn = delegate { };
    public static event Action<NetworkPlayer> OnPlayerSpawned = delegate { };
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playersContainer;
    [SerializeField] private GameObject objectsContainer;
    public WeaponsPrefabs weaponsPrefabs;
    private SocketHandler socketHandler;
    private CameraHandler cameraHandler;
    private Dictionary<string, NetworkPlayer> players = new Dictionary<string, NetworkPlayer>();
    private ServerState serverState;

    private void Awake()
    {
        socketHandler = FindObjectOfType<SocketHandler>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        SocketHandler.OnPlayerJoinedServer += onPlayerJoinedServer;
        SocketHandler.OnPlayerLeftServer += onPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected += onPlayerDisconnected;
    }

    private void Start()
    {
        NetworkGameManager.OnServerStateChanged += onServerStateChanged;
        SocketHandler.OnLatency += onLatency;
        SocketHandler.OnInput += onInput;
        SocketHandler.OnUpdateTransform += onUpdateTransform;
        SocketHandler.OnCharacterRotate += onCharacterRotate;
        SocketHandler.OnSpawn += onSpawn;
        SocketHandler.OnSpawnObject += onSpawnObject;
    }

    public void setPlayers(List<InGamePlayerInfo> inGamePlayersInfos)
    {
        foreach (var inGamePlayerInfo in inGamePlayersInfos)
        {
            setPlayer(inGamePlayerInfo);
        }
    }

    private void setPlayer(InGamePlayerInfo inGamePlayerInfo)
    {
        string playerId = inGamePlayerInfo.id;
        NetworkPlayer player;
        if (!players.TryGetValue(inGamePlayerInfo.id, out player)) {
            GameObject parentObject = new GameObject(playerId);
            parentObject.transform.parent = playersContainer.transform;
            GameObject playerGO = Instantiate(playerPrefab, parentObject.transform);
            player = playerGO.GetComponent<NetworkPlayer>();
            playerGO.name = inGamePlayerInfo.name;
            NetworkIdentity playerNI = playerGO.GetComponent<NetworkIdentity>();
            playerNI.setControllerId(playerId);
            playerNI.setSocket(socketHandler);
            players.Add(playerId, player);
        }
        
        if (serverState == ServerState.inGame && inGamePlayerInfo.playerState == PlayerState.Alive)
        {
            player.init(inGamePlayerInfo, weaponsPrefabs);
            player.enablePlayer(true, player.id.Equals(User.currentUser().id));   
        }
        else
        {
            player.semiInit(inGamePlayerInfo);
        }
    }

    public void disableAllPlayers()
    {
        foreach (var item in players)
        {
            item.Value.enablePlayer(false, true);
        }
    }

    public NetworkPlayer getPlayer(string id)
    {
        if (players.TryGetValue(id, out NetworkPlayer networkPlayer))
        {
            return networkPlayer;
        }
        return null;
    }

    private void onServerStateChanged(ServerState state)
    {
        serverState = state;
    }

    #region Network listening
    private void onLatency(JSONObject jSONObject)
    {
        if (players.TryGetValue(jSONObject["id"].str, out NetworkPlayer player))
        {
            player.setPing((int)jSONObject["latency"].n);
        }
    }

    private void onInput(JSONObject jSONObject)
    {
        if (players.TryGetValue(jSONObject["id"].str, out NetworkPlayer networkPlayer))
        {
            var input = (NetworkInputType)(int)jSONObject["type"].n;
            switch (input) {
                case NetworkInputType.SwitchWeapon:
                    if (jSONObject.HasField("index"))
                    {
                        var index = (int)jSONObject["index"].n;
                        networkPlayer.getLoadoutHolder().switchWeapon(index);
                    } else
                    {
                        networkPlayer.getLoadoutHolder().switchWeapon();
                    }
                    break;
            }
        }
    }

    private void onUpdateTransform(JSONObject jSONObject)
    {
        if (players.TryGetValue(jSONObject["id"].str, out NetworkPlayer player))
        {
            player.setTransform(JsonUtility.FromJson<PlayerTransform>(jSONObject["transform"].ToString()));
        }
    }

    private void onCharacterRotate(JSONObject jSONObject)
    {
        if (players.TryGetValue(jSONObject["id"].str, out NetworkPlayer player))
        {
            player.setCharacterRotation(JsonUtility.FromJson<Vector3>(jSONObject["localRotation"].ToString()));
        }
    }
    private void onSpawn(List<InGamePlayerInfo> inGamePlayerInfos)
    {
        foreach (var item in inGamePlayerInfos)
        {
            NetworkPlayer player = players[item.id];
            player.init(item, weaponsPrefabs);
            bool isSelf = player.id.Equals(User.currentUser().id);
            player.enablePlayer(true, isSelf);
            OnPlayerSpawned(player);
            if (isSelf)
            {
                OnSelfSpawn(player);
                cameraHandler.SwitchCamera(player.playerObjects.firstPersonCamera);//TODO: Decide if third or first person by customization or ingame,  NO: only first person, third is for the stupid
            }
        }
    }

    private void onPlayerJoinedServer(InGamePlayerInfo obj)
    {
        setPlayer(obj);
    }

    private void onPlayerLeftServer(InGamePlayerInfo obj)
    {
        onPlayerDisconnected(obj.id);
    }

    private void onPlayerDisconnected(string playerID)
    {
        NetworkPlayer player = players[playerID];
        Destroy(player.transform.parent.gameObject);
        players.Remove(playerID);
    }

    private void onSpawnObject(JSONObject jSONObject)
    {
        string name = jSONObject["name"].str;//TODO: Maybe change it to ID
        switch (name)
        {
            case ServerObjectName.bullet:
                Bullet bulletObj = GameObjectsPool.instance.get(PoolObjectID.Bullet) as Bullet;
                NetworkObjectsParser.parseBullet(bulletObj, jSONObject);
                if (players.TryGetValue(bulletObj.info.Value.ownerID, out NetworkPlayer networkPlayer)) {
                    if (networkPlayer.getLoadoutHolder().currentActiveWeapon.getWeaponInfo().getID().Equals(bulletObj.info.Value.weaponID))
                    {
                        ((NetworkWeapon)networkPlayer.getLoadoutHolder().currentActiveWeapon).fakeFireBullet();
                    }
                }
                break;
        }
        
    }
    #endregion

    void OnDestroy()
    {
        NetworkGameManager.OnServerStateChanged -= onServerStateChanged;
        SocketHandler.OnPlayerJoinedServer -= onPlayerJoinedServer;
        SocketHandler.OnLatency -= onLatency;
        SocketHandler.OnInput -= onInput;
        SocketHandler.OnUpdateTransform -= onUpdateTransform;
        SocketHandler.OnCharacterRotate -= onCharacterRotate;
        SocketHandler.OnSpawn -= onSpawn;
        SocketHandler.OnSpawnObject -= onSpawnObject;
        SocketHandler.OnPlayerLeftServer -= onPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected -= onPlayerDisconnected;
    }
}
