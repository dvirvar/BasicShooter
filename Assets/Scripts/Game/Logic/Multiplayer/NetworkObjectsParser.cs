using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Parsing the network objects
/// </summary>
[SerializeField]
public class NetworkObjectsParser
{
    public static void parseBullet(Bullet bullet, JSONObject bulletJSON)
    {
        JSONObject transform = bulletJSON["transform"];
        bullet.transform.position = JsonUtility.FromJson<Vector3>(transform["position"].ToString());
        bullet.transform.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(transform["rotation"].ToString()));
        bullet.info = JsonUtility.FromJson<BulletInfo>(bulletJSON["bulletInfo"].ToString());
        bullet.rb.AddForce(JsonUtility.FromJson<Vector3>(bulletJSON["force"].ToString()));
    }
    
}

