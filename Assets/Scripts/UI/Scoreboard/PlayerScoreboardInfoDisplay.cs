using UnityEngine;
using UnityEngine.UI;
//Maybe change it to InGameScoreboardInfoDosplay
public abstract class PlayerScoreboardInfoDisplay<T> : Display<T> where T: PlayerScoreboardInfo
{

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Text nameText, killText, deathText, pingText;

    protected override void setView(T info)
    {
        nameText.text = info.name;
        killText.text = info.kill.ToString();
        deathText.text = info.death.ToString();
        if (info.color != null)
        {
            setColor(info.color.Value);
        }
        pingText.text = info.latency.ToString();
    }

    private void setColor(Color color)
    {
        backgroundImage.color = color;
    }
}