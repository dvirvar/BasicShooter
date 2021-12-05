using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditGameModeInfoVC : ViewController
{
    [SerializeField] private GameModeInfoView[] gameModeInfoViews;
    [SerializeField] private Button editGameBtn;
    [SerializeField] private Popup popup;
    private ServersSocketService serversSocketService;
    private JSONObject editServerData;
    private GameModeType gameModeType;
    private GameModeInfoView currentInfoView => gameModeInfoViews[(int)gameModeType];
    void Start()
    {
        serversSocketService = new ServersSocketService(FindObjectOfType<SocketHandler>());
        editGameBtn.onClick.AddListener(delegate
        {
            var isValidPlusReason = currentInfoView.isValidPlusReason();
            if (isValidPlusReason.valid)
            {
                serversSocketService.editServer(completeEditServerData());
            }
            else
            {
                popup.showCloseBtnOnEnabled = true;
                popup.showWith(isValidPlusReason.reason);
            }
        });
    }

    public void init(WorldState worldState)
    {
        foreach (var item in gameModeInfoViews)
        {
            item.fillData(worldState);
        }
    }

    public void setCreateServerData(JSONObject editServerData)
    {
        this.editServerData = editServerData;
    }

    private JSONObject completeEditServerData()
    {
        editServerData.AddField("gameModeInfo", currentInfoView.createGameModeInfoData());
        return editServerData;
    }

    public void setGameModeType(GameModeType gameModeType)
    {
        this.gameModeType = gameModeType;
        for (int i = 0; i < gameModeInfoViews.Length; i++)
        {
            gameModeInfoViews[i].gameObject.SetActive(i == (int)gameModeType);
        }
    }

    private void OnDestroy()
    {
        editGameBtn.onClick.RemoveAllListeners();
    }
}
