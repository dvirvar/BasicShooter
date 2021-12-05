using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of available server maps
/// </summary>
[CreateAssetMenu(fileName = "Server Maps", menuName = "Server/Maps")]
public class NetworkServerMaps : ScriptableObject
{
    public List<ServerMap> serverMaps;
    public GameObject getMap(MapType name)
    {
        return serverMaps.Find(serverMap => serverMap.name == name).prefab;
    }
}

[System.Serializable]
public class ServerMap
{
    public MapType name;
    public GameObject prefab;
}
