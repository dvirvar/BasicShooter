using UnityEngine;
using System;

/// <summary>
/// Handles logic of character socket requests
/// </summary>
public class CharacterSocketService: BasicSocketService
{

    public CharacterSocketService(SocketHandler socketHandler): base(socketHandler)
    {
        
    }

    public void customizeDonut(Color jellyColor, Color donutColor, Action<BasicSocketResponse> callBack)
    {
        PlayerCustomizationData playerCustomizationData = new PlayerCustomizationData(jellyColor, donutColor);
        JSONObject data = playerCustomizationData.asSocketJson();
        socketHandler.EmitWithToken("customizePlayer", data, delegate (JSONObject json)
         {
             BasicSocketResponse response = new BasicSocketResponse(json.list[0]);
             if (response.parsedResponse.permission)
             {
                 User.currentUser().playerCustomization.jellyColor = playerCustomizationData.jellyColor;
                 User.currentUser().playerCustomization.donutColor = playerCustomizationData.donutColor;
             }
             callBack(response);
         });
    }
}
