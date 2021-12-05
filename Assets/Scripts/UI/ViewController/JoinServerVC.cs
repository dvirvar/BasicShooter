using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinServerVC : ViewController
{
    [SerializeField] private ServersInfoTableView serversTableView;
    [SerializeField] private ServersFilter serversFilter;
    [SerializeField] private Popup popup;
    [SerializeField] private Button refreshBtn;
    private Animator refreshBtnAnim;

    private ServersSocketService serversSocketService;

    private void Awake()
    {
        serversSocketService = new ServersSocketService(FindObjectOfType<SocketHandler>());
        ServerInfoDisplay.OnClick += ServerInfoDisplay_OnClick;
        serversFilter.onFilterChange += serversFilter_onFilterChange;
        refreshBtnAnim = refreshBtn.GetComponent<Animator>();
        refreshBtn.onClick.AddListener(delegate {
            if (!refreshBtnAnim.GetBool("play"))
            {
                AnimateRefresh(true);
                getServers();
            }
        });
    }

    private void serversFilter_onFilterChange(List<ServerInfo> obj)
    {
        serversTableView.setInfos(obj);
    }

    private void ServerInfoDisplay_OnClick(ServerInfo obj)
    {
        serversSocketService.joinServer(obj.id, delegate (JoinServerResponse response)
         {
             if (response.parsedResponse.permission)
             {
                 SceneManager.LoadSceneAsync("Multiplayer");
             }
             else
             {
                 popup.setText(response.parsedResponse.reason);
                 popup.showCloseBtn();
             }
         });
        popup.showWith("Joining server");
    }

    private void OnEnable()
    {
        getServers();
    }

    private void getServers()
    {
        serversSocketService.getServers(delegate (ServersResponse response)
        {
            if (response.parsedResponse.permission)
            {
                serversFilter.serversInfo = response.parsedResponse.servers;
            }
            AnimateRefresh(false);
        });
    }

    private void AnimateRefresh(bool isAnimate)
    {
        refreshBtnAnim.SetBool("play", isAnimate);
    }

    private void OnDestroy()
    {
        ServerInfoDisplay.OnClick -= ServerInfoDisplay_OnClick;
        serversFilter.onFilterChange -= serversFilter_onFilterChange;
    }
}
