using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represent the object from the socket
/// </summary>
[SerializeField]
public class BaseSocketDTO
{
    public bool permission;
    public string reason;
}

public class SocketPermissionStatusDTO: BaseSocketDTO
{
    public LoginInfoDto loginInfo;
}
public class LatencySelfDTO: BaseSocketDTO
{
    public int latency;
}
[SerializeField]
public class LoginInfoDto
{
    public WeaponInfo[] weapons;
    public PlayerCustomization playerCustomization;
    public PlayerLoadoutDTO[] loadouts;
    public string lobbyId;
}

public class ServersDTO: BaseSocketDTO
{
    public List<ServerInfo> servers;
}

public class ServerInfoDTO: BaseSocketDTO
{
    public ServerInfo serverInfo;
}

public class CreateServerStatusDTO: BaseSocketDTO
{
    public ServerInfo serverInfo;
}

public class JoinServerStatusDTO: BaseSocketDTO
{
    public ServerInfo serverInfo;
}

public class SpawnResponseDTO : BaseSocketDTO 
{
    public Spawn spawn;
}