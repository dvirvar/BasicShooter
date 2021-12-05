
public class PreGamePlayersTableView : TableView <InGamePlayerInfo,InGamePlayerDisplay>
{
    public void removeInfoByID(string id) {
        for (int i = 0; i < infosDisplay.Count; i++)
        {
            if (infosDisplay[i].info.id.Equals(id))
            {
                removeInfoAt(i);
                return;
            }
        }
    }

    public void setLatency(string id, int latency)
    {
        foreach (var item in infosDisplay)
        {
            if (item.info.id.Equals(id))
            {
                item.info.latency = latency;
                item.refreshDisplay();
                return;
            }
        }
    }
}
