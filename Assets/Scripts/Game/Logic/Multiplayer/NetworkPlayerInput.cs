using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkPlayerInput : PlayerInput
{
    private NetworkIdentity networkIdentity;

    protected override void Awake()
    {
        base.Awake();
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    protected override void switchWeapon()
    {
        if (loadoutHolder.switchWeapon())
        {
            networkIdentity.getSocket().Emit("input", buildSwitchWeaponJSON(null));
        }
    }

    protected override void switchWeapon(int index)
    {
        if (loadoutHolder.switchWeapon(index))
        {
            networkIdentity.getSocket().Emit("input", buildSwitchWeaponJSON(index));
        }
    }

    private JSONObject buildSwitchWeaponJSON(int? index)
    {
        var data = JSONObject.obj;
        data.AddField("id", networkIdentity.getId());
        data.AddField("type", (int)NetworkInputType.SwitchWeapon);
        if (index.HasValue)
        {
            data.AddField("index", index.Value);
        }
        return data;
    }
}
