using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Handles the network logic of the weapon
/// </summary>
public class NetworkWeapon : Weapon
{
    private NetworkIdentity networkIdentity;

    public void setNetworkIdentity(NetworkIdentity networkIdentity)
    {
        ownerID = networkIdentity.getId();
        this.networkIdentity = networkIdentity;
    }

    protected override void fireBullet(Bullet bullet)
    {
        networkIdentity.getSocket().Emit("spawnObject",createBulletJSON(bullet));
        base.fireBullet(bullet);
    }

    public void fakeFireBullet()
    {
        audioSource.Play();
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
        currentRecoil += weaponStats.recoil;
    }

    private JSONObject createBulletJSON(Bullet bullet)
    {
        JSONObject data = JSONObject.obj;
        data.AddField("name", bullet.name); //TODO: Maybe change it to ID
        JSONObject transform = JSONObject.obj;
        transform.AddField("position", JSONConvertUtil.vector3(bullet.transform.position));
        transform.AddField("rotation", JSONConvertUtil.vector3(bullet.transform.rotation.eulerAngles));
        data.AddField("transform", transform);
        data.AddField("bulletInfo", new JSONObject(JsonConvert.SerializeObject(bullet.info.Value)));
        data.AddField("force", JSONConvertUtil.vector3(fireTransform.forward * weaponStats.firePower));
        return data;
    }
}
