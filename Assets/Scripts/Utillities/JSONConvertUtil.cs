using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class JSONConvertUtil
{
    public static JSONObject vector3(Vector3 vector3)
    {
        JSONObject jsonObject = JSONObject.obj;
        jsonObject.AddField("x", vector3.x);
        jsonObject.AddField("y", vector3.y);
        jsonObject.AddField("z", vector3.z);
        return jsonObject;
    }
}
