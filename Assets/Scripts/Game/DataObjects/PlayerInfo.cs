using System;

public class PlayerInfo: IEquatable<PlayerInfo>
{
    public string id;
    public string name;
    public PlayerCustomization characterCustomization;
    public PlayerLoadouts playerLoadouts;

    public bool Equals(PlayerInfo other)
    {
        return this.id == other.id;
    }
}
