using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicSocketService
{
    protected SocketHandler socketHandler;
    public BasicSocketService(SocketHandler socketHandler)
    {
        this.socketHandler = socketHandler;
    }
}
