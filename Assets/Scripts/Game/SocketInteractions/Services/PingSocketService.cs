using System;

public class PingSocketService : BasicSocketService
{
    public PingSocketService(SocketHandler socketHandler) : base(socketHandler)
    {

    }

    public void latencySelf(Action<LatencySelfResponse> callback)
    {

        var data = JSONObject.obj;
        data.AddField("id", User.currentUser().id);
        data.AddField("latency", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());
        socketHandler.Emit("latencySelf", data, delegate (JSONObject json)
        {
            var response = new LatencySelfResponse(json.list[0]);
            callback(response);
        });
    }
}
