using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPointsScoreboardInfoDisplay : PlayerScoreboardInfoDisplay<PlayerPointsScoreboardInfo>
{
    [SerializeField] private Text pointsText;

    protected override void setView(PlayerPointsScoreboardInfo info)
    {
        base.setView(info);
        pointsText.text = info.points.ToString();
    }
}