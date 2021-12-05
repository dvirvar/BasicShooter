using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class MapsFilter : Filter<MapType, MapFilterRow>
{
    public override List<ServerInfo> filter(List<ServerInfo> serversInfo)
    {
        if (selectedTypes.Count == 0)
        {
            return serversInfo;
        }
        return serversInfo.FindAll(serverInfo =>
        {
            foreach (MapType mapType in selectedTypes)
            {
                if (mapType == serverInfo.map)
                {
                    return true;
                }
            }
            return false;
        });
    }
}
