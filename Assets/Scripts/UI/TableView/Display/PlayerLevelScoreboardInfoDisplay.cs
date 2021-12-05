using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelScoreboardInfoDisplay : PlayerScoreboardInfoDisplay<PlayerLevelScoreboardInfo>
{
    [SerializeField] private Text levelText;
    protected override void setView(PlayerLevelScoreboardInfo info)
    {
        base.setView(info);
        levelText.text = info.level.ToString();
    }
}
