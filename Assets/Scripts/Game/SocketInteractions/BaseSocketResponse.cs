using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Represent the response from the socket
/// </summary>
/// <typeparam name="T">To be parsed from json into T</typeparam>
public class BaseSocketResponse<T> where T : BaseSocketDTO
{
    public T parsedResponse { get; private set; }

    public BaseSocketResponse(JSONObject jSONObject)
    {
        parsedResponse = JsonConvert.DeserializeObject<T>(jSONObject.ToString());
    }
}

public class BasicSocketResponse: BaseSocketResponse<BaseSocketDTO>
{
    public BasicSocketResponse(JSONObject jSONObject) : base(jSONObject)
    {
    }
}

public class SocketPermissionStatusResponse : BaseSocketResponse<SocketPermissionStatusDTO>
{
    public SocketPermissionStatusResponse(JSONObject jSONObject) : base(jSONObject)
    {
        
    }
}

public class LatencySelfResponse: BaseSocketResponse<LatencySelfDTO>
{
    public LatencySelfResponse(JSONObject jSONObject) : base(jSONObject)
    {

    }
}

public class ServersResponse: BaseSocketResponse<ServersDTO>
{
    public ServersResponse(JSONObject jSONObject) : base(jSONObject)
    {

    }
}

public class ServerInfoResponse: BaseSocketResponse<ServerInfoDTO>
{
    public ServerInfoResponse(JSONObject jSONObject) : base(jSONObject)
    {
    }
}

public class CreateServerStatusResponse: BaseSocketResponse<CreateServerStatusDTO>
{
    public CreateServerStatusResponse(JSONObject jSONObject) : base(jSONObject)
    {

    }
}

public class JoinServerResponse : BaseSocketResponse<JoinServerStatusDTO>
{
    public JoinServerResponse(JSONObject jSONObject) : base(jSONObject)
    {

    }
}

public class SpawnResponse : BaseSocketResponse<SpawnResponseDTO>
{
    public SpawnResponse(JSONObject jSONObject) : base(jSONObject)
    { 
    
    }
}