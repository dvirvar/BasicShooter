using UnityEngine;
using System;

[Serializable]
public abstract class PlayerScoreboardInfo
{
    public string id;
    public string name;
    public int teamID;
    public Color? color;
    public int kill;
    public int death;
    public int latency;
}

[Serializable]
public class PlayerPointsScoreboardInfo: PlayerScoreboardInfo, IEquatable<PlayerPointsScoreboardInfo>
{
    public int points;

    public bool Equals(PlayerPointsScoreboardInfo other)
    {
        return id.Equals(other.id);
    }
}

[Serializable] 
public class PlayerLevelScoreboardInfo: PlayerScoreboardInfo, IEquatable<PlayerLevelScoreboardInfo>
{
    public int level = 1;

    public bool Equals(PlayerLevelScoreboardInfo other)
    {
        return id.Equals(other.id);
    }
}