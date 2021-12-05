using System;
[Serializable]
public class InGamePlayerInfo: IEquatable<InGamePlayerInfo>
{
    public string id;
    public string name;
    public PlayerTransform transform;
    public Character character;
    public int latency;
    public int teamID;
    public PlayerState playerState;
    public int health;
    public Loadout loadout;

    public bool Equals(InGamePlayerInfo other)
    {
        return this.id == other.id;
    }
}