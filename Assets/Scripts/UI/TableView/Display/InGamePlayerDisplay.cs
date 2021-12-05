using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerDisplay : Display<InGamePlayerInfo>
{
    [SerializeField] private Text nameText, pingText;

    protected override void setView(InGamePlayerInfo info)
    {
        nameText.text = info.name;
        pingText.text = info.latency.ToString();
    }
}
