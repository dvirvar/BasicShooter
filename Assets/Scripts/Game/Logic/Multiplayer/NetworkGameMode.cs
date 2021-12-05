using UnityEngine;

public class NetworkGameMode : MonoBehaviour
{
    protected SocketHandler socketHandler;
    // Start is called before the first frame update

    protected virtual void Awake()
    {
        socketHandler = FindObjectOfType<SocketHandler>();
    }
}
