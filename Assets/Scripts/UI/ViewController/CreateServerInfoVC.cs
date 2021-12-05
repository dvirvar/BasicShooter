using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CreateServerInfoVC : ViewController
{
    [SerializeField] private Text titleText;
    [SerializeField] private InputField serverNameIF;
    [SerializeField] private InputField maxPlayersIF;
    [SerializeField] private Dropdown mapDropdown;
    [SerializeField] private Dropdown gameModeDropdown;
    [SerializeField] private Dropdown numberOfTeamsDropdown;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Popup popup;
    [SerializeField] private NetworkServerMaps networkServerMaps;
    [SerializeField] private CreateGameModeInfoVC createGameModeInfoController;
    private readonly Dropdown.OptionData pleaseChooseOptionData = new Dropdown.OptionData("Please choose max players first");
    private readonly Dropdown.OptionData firstOptionData = new Dropdown.OptionData("Free For All");

    void Start()
    {
        maxPlayersIF.onEndEdit.AddListener(text =>
        {
            int maxPlayers;
            numberOfTeamsDropdown.options.Clear();
            if (int.TryParse(text, out maxPlayers))
            {
                if (maxPlayers < 2)
                {
                    numberOfTeamsDropdown.options.Add(pleaseChooseOptionData);
                }
                else
                {
                    numberOfTeamsDropdown.options.Add(firstOptionData);
                    for (int i = 2; i < maxPlayers; i++)
                    {
                        if (maxPlayers % i == 0)
                        {
                        numberOfTeamsDropdown.options.Add(new Dropdown.OptionData(i.ToString()));
                        }
                    }
                }
            } else
            {
                numberOfTeamsDropdown.options.Add(pleaseChooseOptionData);
            }
            numberOfTeamsDropdown.RefreshShownValue();
        });

        fillDropDowns();

        nextBtn.onClick.AddListener(delegate
        {
            int maxPlayers;
            if (serverNameIF.text.Trim() == "" || maxPlayersIF.text == "")
            {
                popup.showWith("Fields must not be empty");
            } else if (int.TryParse(maxPlayersIF.text, out maxPlayers))
            {
                if (maxPlayers < 2)
                {
                    popup.showWith("Max players must be between 2-32");
                } else
                {
                    //OnSuccess
                    createGameModeInfoController.setCreateServerData(createServerData());
                    createGameModeInfoController.setGameModeType((GameModeType)gameModeDropdown.value);
                    navigationController.push(createGameModeInfoController);
                }
            } 
        });
    }

    private void OnEnable()
    {
        titleText.text = "Server Info";
    }

    private void fillDropDowns() {
        gameModeDropdown.AddOptions(EnumUtil.getListOfDescriptions<GameModeType>());
        mapDropdown.AddOptions(EnumUtil.getListOfDescriptions<MapType>());
    }

    private JSONObject createServerData()
    {
        int maxPlayers = int.Parse(maxPlayersIF.text);
        JSONObject data = JSONObject.obj;
        data.AddField("serverID", User.currentUser().id);
        data.AddField("hostID", User.currentUser().id);
        data.AddField("name", serverNameIF.text);
        data.AddField("maxPlayers", maxPlayers);
        JSONObject serverMapJSON = JSONObject.obj;
        serverMapJSON.AddField("id", mapDropdown.value);
        List<Spawn> spawns = networkServerMaps.getMap((MapType)mapDropdown.value).GetComponentInChildren<SpawnsInfo>().getSpawns();
        serverMapJSON.AddField("spawns", JSONObject.Create(JsonConvert.SerializeObject(spawns)));
        data.AddField("map", serverMapJSON);
        data.AddField("gameModeType", gameModeDropdown.value);
        if (!int.TryParse(numberOfTeamsDropdown.getCurrentValue(), out int numOfTeams))
        {
            numOfTeams = maxPlayers;//Free for all
        }
        data.AddField("numberOfTeams", numOfTeams);
        return data;
    }

    private void OnDestroy()
    {
        maxPlayersIF.onEndEdit.RemoveAllListeners();
        nextBtn.onClick.RemoveAllListeners();
    }
    
}
