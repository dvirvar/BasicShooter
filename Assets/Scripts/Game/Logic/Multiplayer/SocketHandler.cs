using SocketIO;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// Listens to every network logic
/// </summary>
public class SocketHandler : SocketIOComponent
{
    public static event Action OnSocketOpen = delegate { };
    public static event Action<ushort> OnSocketClose = delegate { };
    public static event Action<int> OnPing = delegate { };
    public static event Action<string> OnPlayerDisconnected = delegate { };
    public static event Action<JSONObject> OnEndGame = delegate { };
    public static event Action<InGamePlayerInfo> OnPlayerJoinedServer = delegate { };
    public static event Action<InGamePlayerInfo> OnPlayerLeftServer = delegate { };
    public static event Action OnLeaveRequest = delegate { };
    public static event Action<ServerInfo> OnServerInfo = delegate { };
    public static event Action<List<InGamePlayerInfo>> OnSpawn = delegate { };
    public static event Action<SpawnResponse> OnTrySpawn = delegate { };
    public static event Action<JSONObject> OnInput = delegate { };
    public static event Action<JSONObject> OnUpdateTransform = delegate { };
    public static event Action<JSONObject> OnCharacterRotate = delegate { };
    public static event Action<JSONObject> OnSpawnObject = delegate { };
    public static event Action<JSONObject> OnGotDamage = delegate { };
    public static event Action<JSONObject> OnPlayerDied = delegate { };
    public static event Action OnEndKillCam = delegate { };
    public static event Action<JSONObject> OnLatency = delegate { };
    public static event Action<WorldState> OnWorldState = delegate { };
    public static event Action<JSONObject> OnNewLoadout = delegate { };

    public static string lobbyID;

    new void Awake()
    {
        base.Awake();
        autoConnect = false;
        url = $"{StaticStrings.Server.socket}";
    }

    new void Start()
    {
        base.Start();
        #region Socket
        socket.OnOpen += onSocketOpen;
        socket.OnClose += onSocketClose;
        #endregion
        #region BasicServer
        On("playerDisconnected", onPlayerDisconnected);
        #endregion
        #region GameServer
        On("joinedServer", onPlayerJoinedServer);
        On("leftServer", onPlayerLeftServer);
        On("leave", onLeaveRequest);
        On("serverInfo", onServerInfo);
        On("spawn", onSpawn);
        On("input", onInput);
        On("updateTransform", onUpdateTransform);
        On("characterRotate", onCharacterRotate);
        On("spawnObject", onSpawnObject);
        On("gotDamage", onGotDamage);
        On("playerDied", onPlayerDied);
        On("endKillCam", onEndKillCam);
        On("latency", onLatency);
        On("worldState", onWorldState);
        On("trySpawn", onTrySpawn);
        On("endGame", onEndGame);
        #endregion
        #region GunMaster
        On("newLoadout", onNewLoadout);
        #endregion
    }

    public void EmitWithToken(string ev, JSONObject data, Action<JSONObject> action)
    {
        if (data == null)
        {
            data = JSONObject.obj;
        }
        data.AddField("token", User.currentUser().token);
        if (action != null)
        {
            base.Emit(ev, data, action);
        } else
        {
            base.Emit(ev, data);
        }
    }
    #region Socket
    private void onSocketOpen(object sender, EventArgs e)
    {
        OnSocketOpen();
    }

    private void onSocketClose(object sender, WebSocketSharp.CloseEventArgs e)
    {
        OnSocketClose(e.Code);
    }
    #endregion
    #region BasicServer
    #region Latency
    private void pingPong()
    {
        var data = JSONObject.obj;
        data.AddField("latency", DateTimeOffset.Now.ToUnixTimeSeconds());
        EmitWithToken("latency", data, delegate (JSONObject json)
        {
            OnPing((int)json.list[0]["latency"].n);
        });
    }
    #endregion

    private void onPlayerDisconnected(SocketIOEvent e)
    {
        OnPlayerDisconnected(e.data["data"].str);
    }

    #endregion
    #region GameServer
    private void onEndGame(SocketIOEvent obj)
    {
        OnEndGame(obj.data);
    }
    private void onPlayerJoinedServer(SocketIOEvent e)
    {
        OnPlayerJoinedServer(JsonConvert.DeserializeObject<InGamePlayerInfo>(e.data["data"].ToString()));
    }

    private void onPlayerLeftServer(SocketIOEvent e)
    {
        OnPlayerLeftServer(JsonConvert.DeserializeObject<InGamePlayerInfo>(e.data["data"].ToString()));
    }

    private void onLeaveRequest(SocketIOEvent e)
    {
        OnLeaveRequest();
    }

    private void onServerInfo(SocketIOEvent e)
    {
        OnServerInfo(JsonConvert.DeserializeObject<ServerInfo>(e.data["data"].ToString()));
    }

    private void onSpawn(SocketIOEvent e)
    {
        OnSpawn(JsonConvert.DeserializeObject<List<InGamePlayerInfo>>(e.data["data"].ToString()));
    }

    private void onTrySpawn(SocketIOEvent e) {
        SpawnResponse response = new SpawnResponse(e.data["data"]);
        OnTrySpawn(response);
    }

    private void onInput(SocketIOEvent e)
    {
        OnInput(e.data);
    }

    private void onUpdateTransform(SocketIOEvent e)
    {
        OnUpdateTransform(e.data);
    }

    private void onCharacterRotate(SocketIOEvent e)
    {
        OnCharacterRotate(e.data);
    }

    private void onSpawnObject(SocketIOEvent e)
    {
        OnSpawnObject(e.data);
    }

    private void onGotDamage(SocketIOEvent e)
    {
        OnGotDamage(e.data);
    }

    private void onPlayerDied(SocketIOEvent e)
    {
        OnPlayerDied(e.data);
    }

    private void onEndKillCam(SocketIOEvent e)
    {
        OnEndKillCam();
    }

    private void onLatency(SocketIOEvent e)
    {
        OnLatency(e.data);
    }

    private void onWorldState(SocketIOEvent e)
    {
        OnWorldState(JsonConvert.DeserializeObject<WorldState>(e.data["data"].ToString()));
    }
    
    #endregion
    #region GunMaster
    private void onNewLoadout(SocketIOEvent e)
    {
        OnNewLoadout(e.data);
    }
    #endregion
    new void OnDestroy() {
        base.OnDestroy();
        socket.OnOpen -= onSocketOpen;
        socket.OnClose -= onSocketClose;
    }
}
