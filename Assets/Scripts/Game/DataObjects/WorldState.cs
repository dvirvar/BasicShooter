using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class WorldState
{
    public List<InGamePlayerInfo> players;
    public ServerInfo serverInfo;
    public ServerGunMasterInfo? gunMasterInfo;
    public ServerDeathmatchInfo? deathmatchInfo;
    public ServerLevelScoreboardInfo? levelScoreboardInfo;
    public ServerPointsScoreboardInfo? pointsScoreboardInfo;

    public void completeScoreboardInfo()
    {
        if (levelScoreboardInfo.HasValue)
        {
            foreach (var item in levelScoreboardInfo.Value.players)
            {
                var player = players.Find(x =>
                {
                    return item.id.Equals(x.id);
                });
                item.name = player.name;
                item.teamID = player.teamID;
            }
        } else if (pointsScoreboardInfo.HasValue)
        {
            foreach (var item in pointsScoreboardInfo.Value.players)
            {
                var player = players.Find(x =>
                {
                    return item.id.Equals(x.id);
                });
                item.name = player.name;
                item.teamID = player.teamID;
            }
        }
    }
}