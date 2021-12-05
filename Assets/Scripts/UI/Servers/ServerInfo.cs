using System;

/// <summary>
/// Represent the static info of a game server
/// </summary>
public class ServerInfo: IEquatable<ServerInfo>
{
    public string id;
    public string hostID;
    public string name;
    public MapType map;
    public GameModeType gameMode;
    public int numberOfTeams;
    public int maxPlayers;
    public int currentPlayers;
    public ServerState state;

    public bool Equals(ServerInfo other)
    {
        return this.id == other.id;
    }

    //public string ping;
    public override string ToString()
    {
        return $"base: {base.ToString()}, id: {id}, hostID: {hostID}, name: {name}, map: {map}, gameMode: {gameMode}, numberOfTeams: {numberOfTeams}, maxPlayers: {maxPlayers}, currentPlayers: {currentPlayers}, state: {state}";
    }
}
