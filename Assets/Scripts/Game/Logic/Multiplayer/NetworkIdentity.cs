using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
/// <summary>
/// Storing the socket object,
/// So you can emit commands,
/// And know who is controlling
/// </summary>
[SerializeField]
public class NetworkIdentity : MonoBehaviour
{
    [SerializeField]
    [GreyOutAtt]
    private string id;
    [SerializeField]
    [GreyOutAtt]
    private bool controlling;

    private SocketIOComponent socket;

    private void Awake()
    {
        controlling = false;
    }

    public void setControllerId(string id)
    {
        this.id = id;
        controlling = User.currentUser().id.Equals(id);
    }

    public void setSocket(SocketIOComponent socket)
    {
        this.socket = socket;
    }

    public string getId()
    {
        return id;
    }

    public SocketIOComponent getSocket()
    {
        return socket;
    }

    public bool isControlling()
    {
        return controlling;
    }
}
