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
            var maxPlayers = serverInfo.maxPlayers;
            foreach (GameSizeFilterType gameSizeFilterType in selectedTypes)
            {
                return gameSizeFilterType switch
                {
                    GameSizeFilterType.TwoToSix => maxPlayers >= 2 && maxPlayers <= 6,
                    GameSizeFilterType.SevenToTwelve => maxPlayers >= 6 && maxPlayers <= 12,
                    GameSizeFilterType.ThirteenToEighteen => maxPlayers >= 13 && maxPlayers <= 18,
                    GameSizeFilterType.ninteenToTwentyFour => maxPlayers >= 19 && maxPlayers <= 24,
                    GameSizeFilterType.TwentyFourToTwentyEight => maxPlayers >= 25 && maxPlayers <= 28,
                    GameSizeFilterType.TwentyNineToThirtyTwo => maxPlayers >= 29 && maxPlayers <= 32,
                    _ => throw new System.NotImplementedException()
                };
            }
            return false;
        });
    }
}

public enum GameSizeFilterType
{
    //Have to be a 4 difference between two numbers in the description
    [Description("2-6")]
    TwoToSix,
    [Description("7-12")]
    SevenToTwelve,
    [Description("13-18")]
    ThirteenToEighteen,
    [Description("19-24")]
    ninteenToTwentyFour,
    [Description("25-28")]
    TwentyFourToTwentyEight,
    [Description("29-32")]
    TwentyNineToThirtyTwo
}
