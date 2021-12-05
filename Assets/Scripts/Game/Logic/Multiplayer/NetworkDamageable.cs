using UnityEngine;
using System;
/// <summary>
/// Handles the network logic of damaging
/// </summary>
//TODO: Apperantly the server should do this and not the client
public class NetworkDamageable: Damageable
{  
    private NetworkIdentity networkIdentity;
    protected override void Awake()
    {
        base.Awake();
        networkIdentity = GetComponentInParent<NetworkIdentity>();
    }

    public override void Damageable_onDamage(BulletInfo info, Bullet bullet)
    {
        if (enabled)
        {
            networkIdentity.getSocket().Emit("gotDamage", createDamageJson(info));
        }
    }
    
    private JSONObject createDamageJson(BulletInfo bulletInfo)
    {
        JSONObject data = JSONObject.obj;
        data.AddField("shooter", bulletInfo.ownerID);
        data.AddField("victim", networkIdentity.getId());
        data.AddField("damage", bulletInfo.damage);
        data.AddField("weaponID", (int)bulletInfo.weaponID);
        return data;
    }
}
