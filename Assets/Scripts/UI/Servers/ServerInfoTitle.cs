using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class ServerInfoTitle : MonoBehaviour
{
    [SerializeField]
    private Text serverNameText;
    [SerializeField]
    private Text gameModeText;
    [SerializeField]
    private Text MapText;
    [SerializeField]
    private Text playersText;
    private int currentPlayers, maxPlayers;
    
    public void setServerInfo(ServerInfo serverInfo)
    {
        serverNameText.text = serverInfo.name;
        string gameModeStr = serverInfo.gameMode.GetDescription();
        if (serverInfo.numberOfTeams != serverInfo.maxPlayers)
        {
            gameModeStr.Insert(0, $"({serverInfo.numberOfTeams}) Team");
        }
        gameModeText.text = gameModeStr;
        MapText.text = serverInfo.map.GetDescription();
        playersText.text = $"{serverInfo.currentPlayers} / {serverInfo.maxPlayers}";
        currentPlayers = serverInfo.currentPlayers;
        maxPlayers = serverInfo.maxPlayers;
    }

    public void addPlayer()
    {
        currentPlayers += 1;
        playersText.text = $"{currentPlayers} / {maxPlayers}";
    }

    public void removePlayer()
    {
        currentPlayers -= 1;
        playersText.text = $"{currentPlayers} / {maxPlayers}";
    }

}
