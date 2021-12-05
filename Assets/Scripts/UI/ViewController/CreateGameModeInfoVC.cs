using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using ExtensionMethods;

public class CreateGameModeInfoVC : ViewController
{
    [SerializeField] private GunMasterCustomizationNC gunMasterCustomizationNC;
    [SerializeField] private GameModeInfoView[] gameModeInfoViews;
    [SerializeField] private Button createGameBtn;
    [SerializeField] private Popup popup;
    private ServersSocketService serversSocketService;
    private JSONObject createServerData;
    private GameModeType gameModeType;
    private GameModeInfoView currentInfoView => gameModeInfoViews[(int)gameModeType];

    private void Awake()
    {
        serversSocketService = new ServersSocketService(FindObjectOfType<SocketHandler>());
        ((GunMasterInfoView)gameModeInfoViews[0]).customizedSelected += CreateGameModeInfoController_customizedSelected;
    }

    private void CreateGameModeInfoController_customizedSelected(string gunMasterWeaponsName)
    {
        navigationController.push(gunMasterCustomizationNC);
        GunMasterWeapons currentGunMasterWeapons = gunMasterWeaponsName == null ? null : UserPrefs.getGunMasterWeapons().Find(gunMasterWeapons =>
        {
            return gunMasterWeapons.name == gunMasterWeaponsName;
        });
        ((GunMasterCustomizationVC)gunMasterCustomizationNC.lastController).setCurrentGunMasterWeapons(currentGunMasterWeapons);
    }

    private void Start()
    {
        createGameBtn.onClick.AddListener(delegate
        {
            var isValidPlusReason = currentInfoView.isValidPlusReason();
            if (isValidPlusReason.valid)
            {
                popup.showCloseBtnOnEnabled = false;
                popup.showWith("Creating game");
                serversSocketService.createServer(completeCreateServerData(), delegate (CreateServerStatusResponse response)
                {
                    if (response.parsedResponse.permission)
                    {
                        SceneManager.LoadSceneAsync("MultiPlayer");
                    }
                    else
                    {
                        popup.setText(response.parsedResponse.reason);
                        popup.showCloseBtn();
                    }
                });
            } else
            {
                popup.showCloseBtnOnEnabled = true;
                popup.showWith(isValidPlusReason.reason);
            }
        });
    }

    public void setCreateServerData(JSONObject createServerData)
    {
        this.createServerData = createServerData;
    }

    private JSONObject completeCreateServerData()
    {
        createServerData.AddField("gameModeInfo", currentInfoView.createGameModeInfoData());
        return createServerData;
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
        createGameBtn.onClick.RemoveAllListeners();
        ((GunMasterInfoView)gameModeInfoViews[0]).customizedSelected -= CreateGameModeInfoController_customizedSelected;
    }
}
