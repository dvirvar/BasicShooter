using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScoreboardTableView : SortedTableView<PlayerLevelScoreboardInfo, PlayerLevelScoreboardInfoDisplay>
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

    public PlayerLevelScoreboardInfo removeInfo(string id)
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

    public void addLevels(string id, int levels)
    {
        addSortPoints(id, levels);
    }

    protected override bool isInfoEqual(string infoId, PlayerLevelScoreboardInfo otherInfo)
    {
        return otherInfo.id.Equals(infoId);
    }

    protected override int addSortPoints(PlayerLevelScoreboardInfo info, int points)
    {
        info.level += points;
        return info.level;
    }

    protected override int sortedPointsOf(PlayerLevelScoreboardInfo info)
    {
        return info.level;
    }
}