using System.Collections.Generic;
using System.ComponentModel;

public class GameSizeFilter : Filter<GameSizeFilterType, GameSizeFilterRow>
{
    public override List<ServerInfo> filter(List<ServerInfo> serversInfo)
    {
        if (selectedTypes.Count == 0)
        {
            return serversInfo;
        }
        return serversInfo.FindAll(serverInfo =>
        {
            foreach (GameSizeFilterType gameSizeFilterType in selectedTypes)
            {
                if ((int)gameSizeFilterType >= serverInfo.maxPlayers && ((int)gameSizeFilterType-4) <= serverInfo.maxPlayers)
                {
                    return true;
                }
            }
            return false;
        });
    }
}

public enum GameSizeFilterType
{
    //Have to be a 4 difference between two numbers in the description
    [Description("2-6")]
    TwoTOSix = 6,
    [Description("8-12")]
    EightTOTwelve = 12,
    [Description("13-17")]
    ThirteenTOSeventeen = 17,
}
