using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGame : MonoBehaviour
{
    [SerializeField] private Button startGameBtn, exitBtn;
    [SerializeField] private ServerInfoTitle serverInfoTitle;
    [SerializeField] private PreGamePlayersTableView playersTableView;
    [SerializeField] private Popup popup;

    private ServersSocketService serversSocketService;

    private void Awake()
    {
        serversSocketService = new ServersSocketService(FindObjectOfType<SocketHandler>());
        startGameBtn.onClick.AddListener(delegate {
            serversSocketService.startServer(delegate(BasicSocketResponse response){
                if (!response.parsedResponse.permission)
                {
                    popup.showWith(response.parsedResponse.reason);
                } else
                {
                    popup.isActive = false;
                }
            });
        });
        exitBtn.onClick.AddListener(delegate
        {
            serversSocketService.joinServer(SocketHandler.lobbyID, delegate (JoinServerResponse response)
             {
                 print($"permission: {response.parsedResponse.permission}, reason: {response.parsedResponse.reason}");
                 SceneManager.LoadSceneAsync("MainMenu");
             });
        });
    }

    private void OnEnable()
    {
        SocketHandler.OnLatency += SocketHandler_OnLatency;
        SocketHandler.OnPlayerJoinedServer += SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnPlayerLeftServer += SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected += SocketHandler_OnPlayerDisconnected;
    }

    private void SocketHandler_OnLatency(JSONObject obj)
    {
        playersTableView.setLatency(obj["id"].str, (int)obj["latency"].n);
    }

    private void SocketHandler_OnPlayerJoinedServer(InGamePlayerInfo obj)
    {
        serverInfoTitle.addPlayer();
        playersTableView.addInfo(obj);
    }

    private void SocketHandler_OnPlayerLeftServer(InGamePlayerInfo obj)
    {
        serverInfoTitle.removePlayer();
        playersTableView.removeInfo(obj);
    }

    private void SocketHandler_OnPlayerDisconnected(string playerID)
    {
        serverInfoTitle.removePlayer();
        playersTableView.removeInfoByID(playerID);
    }

    private void OnDisable()
    {
        SocketHandler.OnLatency -= SocketHandler_OnLatency;
        SocketHandler.OnPlayerJoinedServer -= SocketHandler_OnPlayerJoinedServer;
        SocketHandler.OnPlayerLeftServer -= SocketHandler_OnPlayerLeftServer;
        SocketHandler.OnPlayerDisconnected -= SocketHandler_OnPlayerDisconnected;
    }

    public void init(WorldState worldState)
    {
        startGameBtn.gameObject.SetActive(worldState.serverInfo.hostID.Equals(User.currentUser().id));
        serverInfoTitle.setServerInfo(worldState.serverInfo);
        playersTableView.setInfos(worldState.players);
    }

    private void OnDestroy()
    {
        startGameBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }
}
