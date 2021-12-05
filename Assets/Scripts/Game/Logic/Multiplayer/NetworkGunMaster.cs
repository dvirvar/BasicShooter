using System.Collections.Generic;
using ExtensionMethods;
using Newtonsoft.Json;

/// <summary>
/// Listens to gun master network logic
/// </summary>
public class NetworkGunMaster : NetworkGameMode
{
    private ServerGunMasterInfo serverGunMasterInfo;
    private NetworkObjectsManager networkObjectsManager;
    private ScoreboardManager scoreboardManager;
    private GunMasterSpawnUI gunMasterSpawnUI;
    private WeaponsPrefabs weaponsPrefabs;

    protected override void Awake()
    {
        base.Awake();
        networkObjectsManager = FindObjectOfType<NetworkObjectsManager>();
        scoreboardManager = FindObjectOfType<ScoreboardManager>();
        weaponsPrefabs = networkObjectsManager.weaponsPrefabs;
    }

    protected void Start()
    {
        SocketHandler.OnPlayerJoinedServer += SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnPlayerLeftServer += SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected += SocketHandler_OnPlayerDisconnected;
        SocketHandler.OnNewLoadout += SocketHandler_OnNewLoadout;
    }

    private void SocketHandler_OnPlayerJoinedServer(InGamePlayerInfo obj)
    {
        addPlayer(obj.id);
    }

    private void SocketHandler_OnPlayerDisconnected(string id)
    {
        removePlayer(id);
    }

    private void SocketHandler_OnPlayerLeftServer(InGamePlayerInfo obj)
    {
        removePlayer(obj.id);
    }

    private void addPlayer(string id)
    {
        gunMasterSpawnUI.updateRow(0, 1, null);
        serverGunMasterInfo.points.Add(id, 0);
    }

    private void removePlayer(string id)
    {
        int index = serverGunMasterInfo.points[id] / serverGunMasterInfo.toLevelUp;
        gunMasterSpawnUI.updateRow(index, -1, null);
        serverGunMasterInfo.points.Remove(id);
    }

    public void init(ServerGunMasterInfo serverGunMasterInfo, GunMasterSpawnUI gunMasterSpawnUI)
    {
        this.serverGunMasterInfo = serverGunMasterInfo;
        this.gunMasterSpawnUI = gunMasterSpawnUI;
        var infos = buildGunMasterRowsInfo(); 
        this.gunMasterSpawnUI.setRows(infos.Item1, infos.Item2);
    }

    private (List<GunMasterRowInfo>, string) buildGunMasterRowsInfo()
    {
        string currentWeaponName = "";
        var weaponsIDs = this.serverGunMasterInfo.weaponsIDs;
        var toLevelUp = this.serverGunMasterInfo.toLevelUp;
        List<GunMasterRowInfo> rows = new List<GunMasterRowInfo>(weaponsIDs.Count);
        for (int i = 0; i < weaponsIDs.Count; i++)
        {
            var row = new GunMasterRowInfo();
            row.level = i + 1;
            row.weaponName = weaponsIDs[i].ToString();
            rows.Add(row);
        }
        foreach (var item in this.serverGunMasterInfo.points)
        {
            var index = item.Value / toLevelUp;
            rows[index].numOfPlayers += 1;
            if (item.Key.Equals(User.currentUser().id))
            {
                rows[index].isSelfHere = true;
                currentWeaponName = rows[index].weaponName;
            }
        } 
        return (rows, currentWeaponName);
    }

    private void SocketHandler_OnNewLoadout(JSONObject obj)
    {
        NetworkPlayer player = networkObjectsManager.getPlayer(obj["id"].str);
        Loadout loadout = JsonConvert.DeserializeObject<Loadout>(obj["loadout"].ToString());
        player.setLoadout(loadout, weaponsPrefabs);
        int previousPoints = serverGunMasterInfo.points[player.id];
        int points = (int)obj["points"].n;
        int previousIndex = previousPoints / serverGunMasterInfo.toLevelUp;
        int index = points / serverGunMasterInfo.toLevelUp;
        serverGunMasterInfo.points[player.id] = points;
        bool isSelf = player.id.Equals(User.currentUser().id);
        gunMasterSpawnUI.updateRow(previousIndex, -1, isSelf ? (bool?)false : null);
        gunMasterSpawnUI.updateRow(index, 1, isSelf ? (bool?)true : null);
        scoreboardManager.addGameModePoints(player.id, points - previousPoints);
    }

    private void OnDestroy()
    {
        SocketHandler.OnPlayerJoinedServer -= SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnPlayerLeftServer -= SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected -= SocketHandler_OnPlayerDisconnected;
        SocketHandler.OnNewLoadout -= SocketHandler_OnNewLoadout;
    }
}
