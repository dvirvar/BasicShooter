using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PointsScoreboardTableView : SortedTableView<PlayerPointsScoreboardInfo, PlayerPointsScoreboardInfoDisplay>
{
    [SerializeField] private Text teamText;
    [SerializeField] private Image backgroundImage;

    protected override Direction direction => Direction.Descending;

    public void init(string title, Color? color)
    {
        teamText.text = title;
        if (color.HasValue)
        {
            backgroundImage.color = color.Value;
        }
    }

    public PlayerPointsScoreboardInfo removeInfo(string id)
    {
        for (int i = 0; i < infosDisplay.Count; i++)
        {
            if (getInfoAt(i).id.Equals(id))
            {
                return removeInfoAt(i);
            }
        }
        return null;
    }

    public void addKill(string id)
    {
        foreach (var item in getDisplays())
        {
            if (item.info.id.Equals(id))
            {
                item.info.kill += 1;
                item.refreshDisplay();
                return;
            }
        }    
    }

    public void addDeath(string id)
    {
        foreach (var item in getDisplays())
        {
            if (item.info.id.Equals(id))
            {
                item.info.death += 1;
                item.refreshDisplay();
                return;
            }
        }
    }

    public void setPing(string id, int ping)
    {
        foreach (var item in getDisplays())
        {
            if (item.info.id.Equals(id))
            {
                item.info.latency = ping;
                item.refreshDisplay();
                return;
            }
        }
    }

    public void addPoints(string id, int points)
    {
        addSortPoints(id, points);
    }

    protected override bool isInfoEqual(string infoId, PlayerPointsScoreboardInfo otherInfo)
    {
        return otherInfo.id.Equals(infoId);
    }

    protected override int addSortPoints(PlayerPointsScoreboardInfo info, int points)
    {
        info.points += points;
        return info.points;
    }

    protected override int sortedPointsOf(PlayerPointsScoreboardInfo info)
    {
        return info.points;
    }
}