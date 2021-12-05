using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of available server game modes
/// </summary>
[CreateAssetMenu(fileName = "Server Game Modes", menuName = "Server/GameModes")]
public class NetworkServerGameModes : ScriptableObject
{
    public List<ServerGameMode> serverGameModes;
    public ServerGameMode getGameMode(GameModeType gameModeType)
    {
        return serverGameModes.Find(serverMap => serverMap.gameModeName == gameModeType);
    }
}

[System.Serializable]
public class ServerGameMode
{
    public GameModeType gameModeName;
    public NetworkGameMode gameModeScript;
    public SpawnUI spawnUIPrefab;
    public GameObject scoreboardPrefab;
}
