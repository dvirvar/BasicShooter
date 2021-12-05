using System;

public class AuthSocketService : BasicSocketService
{
    public AuthSocketService(SocketHandler socketHandler): base(socketHandler)
    {

    }

    public void getPermission(Action<SocketPermissionStatusResponse> callBack)
    {
        JSONObject data = JSONObject.obj;
        data.AddField("name", User.currentUser().name);
        data.AddField("clientPermission", "lkjayseajk1");
        socketHandler.EmitWithToken("socketPermission",data, delegate (JSONObject json)
         {
             var response = new SocketPermissionStatusResponse(json.list[0]);
             if (response.parsedResponse.permission)
             {
                 LoginInfoDto loginInfoDto = response.parsedResponse.loginInfo;
                 SocketHandler.lobbyID = loginInfoDto.lobbyId;
                 User.currentUser().playerCustomization = loginInfoDto.playerCustomization;
                 User.currentUser().setWeapons(loginInfoDto.weapons);
                 var loadoutDtos = loginInfoDto.loadouts;
                 var loadoutDto1 = loadoutDtos[0];
                 var loadoutDto2 = loadoutDtos[1];
                 var loadoutDto3 = loadoutDtos[2];
                 var weaponsDic = User.currentUser().weaponsDictionary;
                 var loadout1 = new Loadout(loadoutDto1.id, weaponsDic[loadoutDto1.primaryweaponid], weaponsDic[loadoutDto1.secondaryweaponid]);
                 var loadout2 = new Loadout(loadoutDto2.id, weaponsDic[loadoutDto2.primaryweaponid], weaponsDic[loadoutDto2.secondaryweaponid]);
                 var loadout3 = new Loadout(loadoutDto3.id, weaponsDic[loadoutDto3.primaryweaponid], weaponsDic[loadoutDto3.secondaryweaponid]);
                 User.currentUser().playerLoadouts = new PlayerLoadouts(loadout1, loadout2, loadout3);
             }
             callBack(response);
         });
    }
}
