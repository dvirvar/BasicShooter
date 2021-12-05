using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/// <summary>
/// Stores all the server requests
/// </summary>
[SerializeField]
public class ServerRequests
{
    readonly string serverUrl = StaticStrings.Server.herokuServer;
    int timeout;

    public ServerRequests(int timeout = 10)
    {
        this.timeout = timeout;
    }

    public IEnumerator login(LoginData loginData, Action<LoginResponse> callBack)
    {
        Dictionary<string,string> data = loginData.parseToDictionary();
        string url = $"{serverUrl}users/login";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url,data))
        {
            webRequest.timeout = this.timeout;
            yield return webRequest.SendWebRequest();
            LoginResponse response = new LoginResponse();
            response.statusCode =  webRequest.responseCode;
            response.rawResponse = webRequest.downloadHandler.text;
            callBack(response);
        }
    }
    public IEnumerator register(RegisterData registerData, Action<RegisterResponse> callBack) {
        Dictionary<string, string> data = registerData.parseToDictionary();
        string url = $"{serverUrl}users/register";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, data))
        {
            webRequest.timeout = this.timeout;
            yield return webRequest.SendWebRequest();
            RegisterResponse response = new RegisterResponse();
            response.statusCode = webRequest.responseCode;
            response.rawResponse = webRequest.downloadHandler.text;
            callBack(response);
        }
    }
}
