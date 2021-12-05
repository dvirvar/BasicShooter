using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

/// <summary>
/// Presenter of how to send objects to server
/// </summary>
public abstract class ServerData {
    public virtual string parseToJson()
    {
        //Fields must be public ( need to find out a better way to do it)
        return JsonUtility.ToJson(this);
    }   

    public Dictionary<string,string> parseToDictionary()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach(FieldInfo fi in GetType().GetFields())
        {
            dictionary[fi.Name] = fi.GetValue(this).ToString();
        }
        return dictionary;
    }
}

public class LoginData: ServerData
{
    public string username;
    public string password;

    public LoginData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class RegisterData : ServerData
{
    public string username;
    public string password;

    public RegisterData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class PlayerCustomizationData: ServerData
{
    public string jellyColor;
    public string donutColor;

    public PlayerCustomizationData(Color jellyColor, Color donutColor)
    {
        this.jellyColor = $"#{ColorUtility.ToHtmlStringRGB(jellyColor)}";
        this.donutColor = $"#{ColorUtility.ToHtmlStringRGB(donutColor)}";
    }

    public JSONObject asSocketJson()
    {
        return JSONObject.Create(JsonConvert.SerializeObject(this));
    }
}