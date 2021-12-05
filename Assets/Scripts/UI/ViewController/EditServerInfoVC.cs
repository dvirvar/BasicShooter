using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using Newtonsoft.Json;

public class EditServerInfoVC : ViewController
{
    [SerializeField] private InputField serverNameIF, maxPlayersIF;
    [SerializeField] private Dropdown mapDropdown, gameModeDropdown, numberOfTeamsDropdown;
    [SerializeField] private Button nextBtn;
    [SerializeField] private NetworkServerMaps networkServerMaps;
    [SerializeField] private EditGameModeInfoVC editGameModeInfoController;
    private bool initialized = false;
    private readonly Dropdown.OptionData firstOptionData = new Dropdown.OptionData("Free For All");
    private int maxPlayers;
    
    private void Start()
    {
        nextBtn.onClick.AddListener(delegate
        {
            editGameModeInfoController.setCreateServerData(createServerData());
            editGameModeInfoController.setGameModeType((GameModeType)gameModeDropdown.value);
            navigationController.push(editGameModeInfoController);
        });
    }

    public void init(WorldState worldState)
    {
        if (!initialized)
        {
            mapDropdown.AddOptions(EnumUtil.getListOfDescriptions<MapType>());
            gameModeDropdown.AddOptions(EnumUtil.getListOfDescriptions<GameModeType>());
            initialized = true;
        }
        var serverInfo = worldState.serverInfo;
        this.maxPlayers = serverInfo.maxPlayers;
        serverNameIF.text = serverInfo.name;
        maxPlayersIF.text = maxPlayers.ToString();
        mapDropdown.value = (int)serverInfo.map;
        gameModeDropdown.value = (int)serverInfo.gameMode;
        
        var numberOfTeamsData = new List<Dropdown.OptionData>();
        numberOfTeamsData.Add(firstOptionData);
        int numberOfTeamsIndex = 0;
        int chosenNumberOfTeams = 0;
        for (int i = 2; i < maxPlayers; i++)
        {
            if (maxPlayers % i == 0)
            {
                numberOfTeamsIndex++;
                numberOfTeamsData.Add(new Dropdown.OptionData(i.ToString()));
                if (serverInfo.numberOfTeams == i)
                {
                    chosenNumberOfTeams = numberOfTeamsIndex;
                }
            }
        }
        numberOfTeamsDropdown.options = numberOfTeamsData;
        numberOfTeamsDropdown.value = chosenNumberOfTeams;
        editGameModeInfoController.init(worldState);
    }

    public JSONObject createServerData() {
        JSONObject data = JSONObject.obj;
        JSONObject serverMapJSON = JSONObject.obj;
        serverMapJSON.AddField("id", mapDropdown.value);
        List<Spawn> spawns = networkServerMaps.getMap((MapType) mapDropdown.value).GetComponentInChildren<SpawnsInfo>().getSpawns();
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
        nextBtn.onClick.RemoveAllListeners();
    }
}
