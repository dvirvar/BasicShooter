using System.Collections;
using System;

public class ServersSocketService : BasicSocketService
{
    public ServersSocketService(SocketHandler socketHandler) : base(socketHandler)
    {

    }

    public void getServers(Action<ServersResponse> callBack)
    {
        socketHandler.EmitWithToken("servers", null, delegate (JSONObject json)
        {
            callBack(new ServersResponse(json.list[0]));
        });
    }

    public void joinServer(string serverId, Action<JoinServerResponse> callBack)
    {
        var data = JSONObject.obj;
        data.AddField("serverID", serverId);
        data.AddField("playerID", User.currentUser().id);
        socketHandler.Emit("joinServer", data, delegate (JSONObject json)
          {
              callBack(new JoinServerResponse(json.list[0]));
          });
    }

    public void createServer(JSONObject data, Action<CreateServerStatusResponse> callBack)
    {
        socketHandler.Emit("createServer", data, delegate (JSONObject json)
        {
            callBack(new CreateServerStatusResponse(json.list[0]));
        });
    }

    public void editServer(JSONObject data)
    {
        data.AddField("id", User.currentUser().id);
        socketHandler.Emit("editGameServer", data);
    }

    public void startServer(Action<BasicSocketResponse> callBack)
    {
        var data = JSONObject.obj;
        data.AddField("id", User.currentUser().id);
        socketHandler.Emit("startGame", data, delegate (JSONObject json)
        {
            callBack(new BasicSocketResponse(json.list[0]));
        });
    }
}
