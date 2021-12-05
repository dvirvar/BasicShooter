using System.Collections.Generic;

/// <summary>
/// Game modes filter
/// </summary>
public class GameModesFilter : Filter<GameModeType,GameModeFilterRow>
{
    public override List<ServerInfo> filter(List<ServerInfo> serversInfo)
    {
        if (selectedTypes.Count == 0)
        {
            return serversInfo;
        }
        return serversInfo.FindAll(serverInfo =>
        {
            foreach(GameModeType gameModeType in selectedTypes)
            {
                if (gameModeType == serverInfo.gameMode)
                {
                    return true;
                }
            }
            return false;
        });
    }

}

