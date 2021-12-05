using System.Collections.Generic;
using System.ComponentModel;

public class FreeSlotsFilter : Filter<FreeSlotsFilterType, FreeSlotsFilterRow>
{
    public override List<ServerInfo> filter(List<ServerInfo> serversInfo)
    {
        if (selectedTypes.Count == 0)
        {
            return serversInfo;
        }
        return serversInfo.FindAll(serverInfo =>
        {
            int freeSlots = serverInfo.maxPlayers - serverInfo.currentPlayers;
            foreach(FreeSlotsFilterType freeSlotsFilterType in selectedTypes)
            {
                return freeSlots >= (int)freeSlotsFilterType;
            }
            return false;
        });
    }
}

public enum FreeSlotsFilterType
{
    [Description("4+")]
    Four = 4,
    [Description("8+")]
    Eight = 8,
    [Description("12+")]
    Twelve = 12,
    [Description("16+")]
    Sixteen = 16,
}
