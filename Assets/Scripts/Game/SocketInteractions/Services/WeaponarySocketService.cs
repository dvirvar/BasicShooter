using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponarySocketService : BasicSocketService
{
    public WeaponarySocketService(SocketHandler socketHandler): base(socketHandler)
    {

    }

    public void customizeLoadouts(PlayerLoadouts playerLoadouts, Action<BasicSocketResponse> callBack)
    {
        socketHandler.EmitWithToken("customizeLoadouts", playerLoadouts.asJsonObject(), delegate (JSONObject json)
        {
            callBack(new BasicSocketResponse(json.list[0]));
        });
    }

    public void customizeWeapon(WeaponInfo weaponInfo, Action<BasicSocketResponse> callBack)
    {
        socketHandler.EmitWithToken("customizeWeapon", weaponInfo.asJSONObject(), delegate (JSONObject json)
        {
            callBack(new BasicSocketResponse(json.list[0]));
        });
    }
}
