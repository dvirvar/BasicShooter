using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: Some logic should be moved to player
public class MinimapController : MonoBehaviour
{
    [SerializeField] private GameObject minimap, minimapCameraPrefab, selfMinimapPres, teamPlayerMinimapPres, enemyPlayerMinimapPres;
    private NetworkObjectsManager networkObjectsManager;
    private HashSet<string> firstTime = new HashSet<string>();

    private void Awake() {
        networkObjectsManager = GetComponent<NetworkObjectsManager>();
        NetworkObjectsManager.OnPlayerSpawned += SpawnHandler_OnPlayerSpawned;
        SocketHandler.OnPlayerDied += SocketHandler_OnPlayerDied;
        SocketHandler.OnPlayerLeftServer += SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected += SocketHandler_OnPlayerDisconnected;
    }

    private void OnDestroy()
    {
        NetworkObjectsManager.OnPlayerSpawned -= SpawnHandler_OnPlayerSpawned;
        SocketHandler.OnPlayerDied -= SocketHandler_OnPlayerDied;
        SocketHandler.OnPlayerLeftServer -= SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected -= SocketHandler_OnPlayerDisconnected;
    }

    private void SpawnHandler_OnPlayerSpawned(NetworkPlayer player)
    {
        var isFirstTime = firstTime.Contains(player.id) ? false : true;
        if (player.id.Equals(User.currentUser().id))
        {
            minimap.SetActive(true);
            if (isFirstTime)
            {
                Instantiate(minimapCameraPrefab, player.transform);
                Instantiate(selfMinimapPres, player.transform);
            }
        }
        else
        {
            if (isFirstTime)
            {
                var selfPlayer = networkObjectsManager.getPlayer(User.currentUser().id);
                if (selfPlayer.teamID == player.teamID)
                {
                    Instantiate(teamPlayerMinimapPres, player.transform);
                } else {
                    Instantiate(enemyPlayerMinimapPres, player.transform);
                }
            }
        }
        if (!firstTime.Contains(player.id))
        {
            firstTime.Add(player.id);
        }
    }


    private void SocketHandler_OnPlayerDied(JSONObject obj)
    {
        if (obj["dead"].str.Equals(User.currentUser().id))
        {
            minimap.SetActive(false);
        }
    }

    private void SocketHandler_OnPlayerDisconnected(string obj)
    {
        firstTime.Remove(obj);
    }

    private void SocketHandler_OnPlayerLeftServer(InGamePlayerInfo obj)
    {
        SocketHandler_OnPlayerDisconnected(obj.id);
    }
}
