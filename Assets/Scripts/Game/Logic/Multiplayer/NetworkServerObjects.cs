using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of available server objects
/// </summary>
[CreateAssetMenu(fileName = "Server Objects", menuName = "Server/Objects")]
public class NetworkServerObjects: ScriptableObject {
    public List<ServerObject> serverObjects;
    public GameObject getPrefab(string name)
    {
        return serverObjects.Find(serverObject => serverObject.name == name).prefab;
    }
}

[System.Serializable]
public class ServerObject
{
    public string name;
    public GameObject prefab;
}

public class ServerObjectName
{
    public const string bullet = "Bullet";
}
