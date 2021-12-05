using UnityEngine;
using UnityEngine.UI;

public abstract class GameModeInfoView : MonoBehaviour
{
    [SerializeField] private Text titleText;

    protected virtual void OnEnable()
    {
        if (titleText != null)
        {
            titleText.text = title();
        }
    }

    protected abstract string title();
    public abstract void fillData(WorldState worldState);
    public abstract (bool valid,string reason) isValidPlusReason();
    public abstract JSONObject createGameModeInfoData();
}
