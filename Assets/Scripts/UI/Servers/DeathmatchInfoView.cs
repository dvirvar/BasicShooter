using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class DeathmatchInfoView : GameModeInfoView
{
    [SerializeField] private InputField pointsToWinIF;

    protected override string title()
    {
        return $"{GameModeType.Deathmatch.GetDescription()} Info";
    }

    public override void fillData(WorldState worldState)
    {
        if (worldState.deathmatchInfo.HasValue)
        {
            var deathmatchInfo = worldState.deathmatchInfo.Value;
            pointsToWinIF.text = deathmatchInfo.pointsToWin.ToString();
        }
    }

    public override JSONObject createGameModeInfoData()
    {
        JSONObject data = JSONObject.obj;
        data.AddField("pointsToWin", int.Parse(pointsToWinIF.text));
        return data;
    }

    public override (bool valid, string reason) isValidPlusReason()
    {
        bool isValid;
        string reason;
        if (int.TryParse(pointsToWinIF.text,out int pointsToWin))
        {
            isValid = pointsToWin >= 50 && pointsToWin <= 250;
            reason = isValid ? "" : "Points to win must be between 50 - 250";
        } else
        {
            isValid = false;
            reason = "Points to win must be between 50 - 250";
        }
        
        return (isValid,reason);
    }
}
