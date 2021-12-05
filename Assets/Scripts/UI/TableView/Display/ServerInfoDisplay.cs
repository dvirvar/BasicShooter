using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ExtensionMethods;

public class ServerInfoDisplay: Display<ServerInfo>
{
    public static event Action<ServerInfo> OnClick = delegate { };
    public Text nameText;
    public Text gameModeText;
    public Text mapText;
    public Text playersText;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            OnClick(info);
        });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    protected override void setView(ServerInfo info)
    {
        nameText.text = info.name;
        gameModeText.text = info.gameMode.GetDescription();
        mapText.text = info.map.GetDescription();
        playersText.text = $"{info.currentPlayers}/{info.maxPlayers}";
    }
}
