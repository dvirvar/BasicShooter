using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInputManager : Input
{
    private static GameInputManager singlton;
    Dictionary<string, string> keyMapping;
    string[] keyMaps = new string[6]
    {
        "Attack",
        "Block",
        "Forward",
        "Backward",
        "Left",
        "Right"
    };

    string[] defaults = new string[6]
    {
        ((int)KeyCode.Q).ToString(),
        ((int)KeyCode.E).ToString(),
        ((int)KeyCode.W).ToString(),
        ((int)KeyCode.S).ToString(),
        ((int)KeyCode.A).ToString(),
        ((int)KeyCode.D).ToString()
    };

    public static GameInputManager getInstance() {
        if (singlton == null) {
            singlton = new GameInputManager();
        }
        return singlton;
    }

    private GameInputManager() {
        InitializeDefaultVars();
        InitializeInputs();
    }

    private void InitializeDefaultVars() {
        keyMapping = new Dictionary<string, string>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    private void InitializeInputs() {
        if (PlayerPrefs.GetString("Inputs") == "")
        {
            //Setting the default values
            PlayerPrefs.SetString("Inputs", JSONObject.Create(keyMapping).ToString());
        }   
    }
   
    /// <param name="key">In Ascii</param>
    public void SetKeyMap(string keyMap, KeyCode key)
    {
        JSONObject jsonInputs = JSONObject.Create(PlayerPrefs.GetString("Inputs"));
        jsonInputs[keyMap] = JSONObject.CreateStringObject(((int)key).ToString());
        PlayerPrefs.SetString("Inputs",jsonInputs.ToString());
        Debug.Log(jsonInputs);
    }

    public bool GetKeyDown(string keyMap)
    {
        JSONObject jsonInputs = JSONObject.Create(PlayerPrefs.GetString("Inputs"));
        return Input.GetKeyDown((KeyCode)int.Parse(jsonInputs[keyMap].ToString()));
    }
    private string toAscii(string value) {
        //return ((int)(value).ToString();
        return "";
    }
}