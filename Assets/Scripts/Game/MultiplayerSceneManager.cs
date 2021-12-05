using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSceneManager : MonoBehaviour
{
    [SerializeField] private InGameMenu menu;
    private ServersSocketService serversSocketService;
    private NetworkPlayer selfPlayer;
    private bool isAlive;
    private ServerState serverState;
    private void Awake()
    {
        FindObjectOfType<AudioHandler>().stopBgAudio();
        serversSocketService = new ServersSocketService(FindObjectOfType<SocketHandler>());
        NetworkGameManager.OnServerStateChanged += NetworkGameManager_OnServerStateChanged;
        NetworkObjectsManager.OnSelfSpawn += NetworkObjectsManager_OnSelfSpawn;
        SocketHandler.OnPlayerDied += SocketHandler_OnPlayerDied;
        SocketHandler.OnLeaveRequest += SocketHandler_OnLeaveRequest;
        menu.OnResumePressed += Menu_OnResumePressed;
        menu.OnExitPressed += Menu_OnExitPressed;
    }    

    private void Menu_OnExitPressed()
    {
        serversSocketService.joinServer(SocketHandler.lobbyID, delegate (JoinServerResponse response)
        {
            print($"permission: {response.parsedResponse.permission}, reason: {response.parsedResponse.reason}");
            SceneManager.LoadSceneAsync("MainMenu");
        });
    }

    private void SocketHandler_OnLeaveRequest()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (serverState == ServerState.preGame)
            {
                return;
            }
            menu.gameObject.SetActive(!menu.gameObject.activeInHierarchy);
            if (serverState == ServerState.inGame)
            {
                enableSelfPlayerInput(!menu.gameObject.activeInHierarchy);
                if (!isAlive)
                {
                    setCursorVisibility(true);
                } else
                {
                    setCursorVisibility(menu.gameObject.activeInHierarchy);
                }
            }
        }
    }

    private void OnDestroy()
    {
        NetworkGameManager.OnServerStateChanged -= NetworkGameManager_OnServerStateChanged;
        NetworkObjectsManager.OnSelfSpawn -= NetworkObjectsManager_OnSelfSpawn;
        SocketHandler.OnLeaveRequest -= SocketHandler_OnLeaveRequest;
        menu.OnResumePressed -= Menu_OnResumePressed;
        menu.OnExitPressed -= Menu_OnExitPressed;
    }

    private void NetworkGameManager_OnServerStateChanged(ServerState obj)
    {
        serverState = obj;
        menu.gameObject.SetActive(false);
        if (serverState != ServerState.inGame)
        {
            setCursorVisibility(true);
        }
    }

    private void NetworkObjectsManager_OnSelfSpawn(NetworkPlayer obj)
    {
        selfPlayer = obj;
        isAlive = true;
    }

    private void SocketHandler_OnPlayerDied(JSONObject obj)
    {
        if (obj["dead"].str.Equals(User.currentUser().id)) {
            isAlive = false;
        }
    }

    private void Menu_OnResumePressed()
    {
        if (serverState == ServerState.inGame)
        {
            setCursorVisibility(!isAlive);
            enableSelfPlayerInput(true);
        }
    }

    private void enableSelfPlayerInput(bool enable)
    {
        if (selfPlayer != null)
        {
            selfPlayer.enablePlayerInput(enable);
        }
    }

    private void setCursorVisibility(bool show)
    {
        if (show)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
