using UnityEngine;
using System;
using System.Collections;
using ExtensionMethods;

public class NetworkGameManager : MonoBehaviour
{
    public static event Action<ServerState> OnServerStateChanged = delegate { };
    [SerializeField] private PreGame preGame;
    [SerializeField] private InGame inGame;
    [SerializeField] private PostGame postGame;
    [SerializeField] private NetworkServerGameModes networkServerGameModes;
    [SerializeField] private NetworkServerMaps networkServerMaps;
    [SerializeField] private GameObject mapContainer;
    [SerializeField] private ScoreboardManager scoreboardManager;
    [SerializeField] private KillsFeedPanel killsFeedPanel;
    [SerializeField] private HitEventsPanel hitEventsPanel;

    private WorldState worldState;
    private SpawnsInfo spawnsInfo;
    private SocketHandler socketHandler;
    private NetworkObjectsManager networkObjectsManager;
    private CameraHandler cameraHandler;
    private MapInfo currentMap;

    private void Awake()
    {
        networkObjectsManager = GetComponent<NetworkObjectsManager>();
        socketHandler = FindObjectOfType<SocketHandler>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    private void Start()
    {
        NetworkObjectsManager.OnSelfSpawn += NetworkObjectsManager_OnSelfSpawn;
        SocketHandler.OnPlayerJoinedServer += SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnWorldState += SocketHandler_OnWorldState;
        SocketHandler.OnLatency += SocketHandler_OnLatency;
        SocketHandler.OnServerInfo += SocketHandler_OnServerInfo;
        SocketHandler.OnTrySpawn += SocketHandler_OnTrySpawn;
        SocketHandler.OnGotDamage += SocketHandler_OnGotDamage;
        SocketHandler.OnPlayerDied += SocketHandler_OnPlayerDied;
        SocketHandler.OnEndKillCam += SocketHandler_OnEndKillCam;
        SocketHandler.OnEndGame += SocketHandler_OnEndGame;
        SocketHandler.OnPlayerLeftServer += SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected += SocketHandler_OnPlayerDisconnected;
        
        socketHandler.Emit("worldState");
    }

    private void getLatency()
    {
        var data = JSONObject.obj;
        data.AddField("id", User.currentUser().id);
        data.AddField("latency", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());
        socketHandler.Emit("latency", data);
    }

    private void setServerInfo(ServerInfo serverInfo)
    {
        this.worldState.serverInfo = serverInfo;
        switch (serverInfo.state)
        {
            case ServerState.preGame:
                SetPreGame();
                break;
            case ServerState.inGame:
                setInGame();
                break;
            case ServerState.postGame:
                setPostGame(null);
                break;
        }
        
        OnServerStateChanged(serverInfo.state);
    }

    private void SetPreGame()
    {
        cameraHandler.stopAnimating();
        int mapsCountInContainer = mapContainer.transform.childCount;
        for (int i = 0; i < mapsCountInContainer; i++)
        {
            DestroyImmediate(mapContainer.transform.GetChild(i).gameObject, true);
        }
        preGame.gameObject.SetActive(true);
        inGame.gameObject.SetActive(false);
        postGame.gameObject.SetActive(false);
        BuildGameLogic();
        preGame.init(worldState);
    }

    private void setInGame()
    {
        preGame.gameObject.SetActive(false);
        inGame.gameObject.SetActive(true);
        postGame.gameObject.SetActive(false);
        BuildGameLogic();
    }

    private void setPostGame(int? timeout)
    {
        try
        {
            preGame.gameObject.SetActive(false);
            inGame.gameObject.SetActive(false);
            postGame.gameObject.SetActive(true);
            showCursor();
            ResetGameLogic();
            postGame.init(worldState, timeout);
        } catch (Exception e)
        {
            print(e);
        }
    }

    private void BuildGameLogic()
    {
        if (mapContainer.transform.childCount == 0)
        {
            currentMap = Instantiate(networkServerMaps.getMap(worldState.serverInfo.map), mapContainer.transform).GetComponent<MapInfo>();
            cameraHandler.AnimateCameras(currentMap.previewsCameras,3f);
            spawnsInfo = FindObjectOfType<SpawnsInfo>();
        }
        if (FindObjectOfType<NetworkGameMode>() == null)
        {
            ServerGameMode serverGameMode = networkServerGameModes.getGameMode(worldState.serverInfo.gameMode);
            Instantiate(serverGameMode.gameModeScript);
            inGame.setSpawnUI(serverGameMode.spawnUIPrefab);
        }
    }

    private IEnumerator setScoreboardManager(GameObject scoreboardPrefab, int numberOfScoreboards)
    {
        scoreboardManager.transform.SetParent(null);
        scoreboardManager.gameObject.SetActive(true);
        yield return scoreboardManager.buildScoreboardsCourotine(numberOfScoreboards, scoreboardPrefab);
        inGame.setScoreboardManager(scoreboardManager);
        if (worldState.levelScoreboardInfo.HasValue)
        {
            scoreboardManager.setPlayers(worldState.levelScoreboardInfo.Value.players.ConvertAll<PlayerScoreboardInfo>(x => x));
        }
        else if (worldState.pointsScoreboardInfo.HasValue)
        {
            scoreboardManager.setPlayers(worldState.pointsScoreboardInfo.Value.players.ConvertAll<PlayerScoreboardInfo>(x => x));
        }
        scoreboardManager.gameObject.SetActive(false);
    }

    private void ResetGameLogic()
    {
        if (mapContainer.transform.childCount == 0)
        {
            currentMap = Instantiate(networkServerMaps.getMap(worldState.serverInfo.map), mapContainer.transform).GetComponent<MapInfo>();
        }
        NetworkGameMode networkGameMode = FindObjectOfType<NetworkGameMode>();
        if (networkGameMode != null)
        {
            Destroy(networkGameMode.gameObject);
        }
        networkObjectsManager.disableAllPlayers();
        cameraHandler.AnimateCameras(currentMap.previewsCameras, 3f);
        killsFeedPanel.removeAll();
        hitEventsPanel.removeAll();
        inGame.cleanUI();
    }

    private void showCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void hideCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    #region Callbacks
    private void NetworkObjectsManager_OnSelfSpawn(NetworkPlayer selfPlayer)
    {
        inGame.showSpawnUI(false);
        hideCursor();
    }

    private void SocketHandler_OnPlayerJoinedServer(InGamePlayerInfo obj)
    {
        if (worldState.levelScoreboardInfo.HasValue)
        {
            var info = new PlayerLevelScoreboardInfo();
            info.id = obj.id;
            info.name = obj.name;
            info.teamID = obj.teamID;
            scoreboardManager.setPlayer(info);
        }
        else if (worldState.pointsScoreboardInfo.HasValue)
        {
            var info = new PlayerPointsScoreboardInfo();
            info.id = obj.id;
            info.name = obj.name;
            info.teamID = obj.teamID;
            scoreboardManager.setPlayer(info);
        }
    }

    private void SocketHandler_OnWorldState(WorldState obj)
    {
        obj.completeScoreboardInfo();
        this.worldState = obj;
        setServerInfo(obj.serverInfo);
        networkObjectsManager.setPlayers(obj.players);
        if (obj.gunMasterInfo.HasValue && obj.serverInfo.state != ServerState.postGame)
        {
            FindObjectOfType<NetworkGunMaster>().init(obj.gunMasterInfo.Value, inGame.getSpawnUI() as GunMasterSpawnUI);
        }
        ServerGameMode serverGameMode = networkServerGameModes.getGameMode(worldState.serverInfo.gameMode);
        int numberOfScoreboards = worldState.serverInfo.maxPlayers == worldState.serverInfo.numberOfTeams ? 1 : worldState.serverInfo.numberOfTeams;
        StartCoroutine(setScoreboardManager(serverGameMode.scoreboardPrefab, numberOfScoreboards));
        if (!IsInvoking("getLatency"))
        {
            getLatency();
        }
    }

    private void SocketHandler_OnLatency(JSONObject obj)
    {
        var id = obj["id"].str;
        try
        {
            scoreboardManager.setPing(id, (int)obj["latency"].n);
            if (id.Equals(User.currentUser().id))
            {
                Invoke("getLatency", 10);
            }
        } catch (Exception e)
        {
            print(e.ToString());
        }
    }

    private void SocketHandler_OnServerInfo(ServerInfo obj)
    {
        setServerInfo(obj);
    }

    private void SocketHandler_OnTrySpawn(SpawnResponse obj)
    {
        if (obj.parsedResponse.permission)
        {
            JSONObject jSONObject = JSONObject.obj;
            jSONObject.AddField("id", User.currentUser().id);
            jSONObject.AddField("position", JSONConvertUtil.vector3(spawnsInfo.GetSpawnHelper(obj.parsedResponse.spawn.id).getFreePoint()));
            jSONObject.AddField("rotation", JSONConvertUtil.vector3(obj.parsedResponse.spawn.rotation));
            socketHandler.Emit("spawn", jSONObject);
        }
        else
        {
            print(obj.parsedResponse.reason);
        }
    }

    private void SocketHandler_OnGotDamage(JSONObject obj)
    {
        var shooterID = obj["shooter"].str;
        if (shooterID.Equals(User.currentUser().id))
        {
            var victim = networkObjectsManager.getPlayer(obj["victim"].str);
            var shooter = networkObjectsManager.getPlayer(shooterID);
            var damage = (int)obj["damage"].n;
            hitEventsPanel.addDamage(victim.name, victim.teamID == shooter.teamID ? -damage : damage);
        }
    }

    private void SocketHandler_OnPlayerDied(JSONObject obj)
    {
        NetworkPlayer dead = networkObjectsManager.getPlayer(obj["dead"].str);
        NetworkPlayer killer = networkObjectsManager.getPlayer(obj["killer"].str);
        #region UI
        scoreboardManager.addDeath(dead.id);
        scoreboardManager.addKill(killer.id);
        killsFeedPanel.addKillFeed(killer.name, dead.name, ((WeaponID)obj["killingWeaponID"].n).ToString());
        if (killer.id.Equals(User.currentUser().id))
        {
            var isTeamKill = killer.teamID == dead.teamID;
            var damage = (int)obj["damage"].n;
            hitEventsPanel.addDamage(dead.name, isTeamKill ? -damage : damage);
            hitEventsPanel.addDeath(dead.name, isTeamKill ? -100 : 100);
        }
        #endregion
        dead.transform.position = new Vector3(9999, 9999, 9999);
        bool isSelf = dead.id.Equals(User.currentUser().id);
        dead.enablePlayer(false, isSelf);
        if (isSelf && worldState.serverInfo.state == ServerState.inGame)
        {
            cameraHandler.SwitchCamera(killer.playerObjects.killCamera, 5f, delegate
            {
                //When finished looking at killcam
                JSONObject data = JSONObject.obj;
                data.AddField("id", User.currentUser().id);
                socketHandler.Emit("endKillCam", data);
            });
        }
    }
    private void SocketHandler_OnEndKillCam()
    {
        inGame.showSpawnUI(true);
        cameraHandler.AnimateCameras(currentMap.previewsCameras, 3f);
        showCursor();
    }

    private void SocketHandler_OnEndGame(JSONObject obj)
    {
        setPostGame((int)obj["data"]["time"].n);
    }

    private void SocketHandler_OnPlayerLeftServer(InGamePlayerInfo obj)
    {
        scoreboardManager.removePlayer(obj.id);
    }

    private void SocketHandler_OnPlayerDisconnected(string id)
    {
        scoreboardManager.removePlayer(id);
    }
    #endregion

    private void OnDestroy()
    {
        CancelInvoke();
        NetworkObjectsManager.OnSelfSpawn -= NetworkObjectsManager_OnSelfSpawn;
        SocketHandler.OnPlayerJoinedServer -= SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnWorldState -= SocketHandler_OnWorldState;
        SocketHandler.OnLatency -= SocketHandler_OnLatency;
        SocketHandler.OnServerInfo -= SocketHandler_OnServerInfo;
        SocketHandler.OnTrySpawn -= SocketHandler_OnTrySpawn;
        SocketHandler.OnGotDamage -= SocketHandler_OnGotDamage;
        SocketHandler.OnPlayerDied -= SocketHandler_OnPlayerDied;
        SocketHandler.OnEndKillCam -= SocketHandler_OnEndKillCam;
        SocketHandler.OnEndGame -= SocketHandler_OnEndGame;        
        SocketHandler.OnPlayerLeftServer -= SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected -= SocketHandler_OnPlayerDisconnected;
    }
}
