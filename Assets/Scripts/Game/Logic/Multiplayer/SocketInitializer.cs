using UnityEngine;

public class SocketInitializer : MonoBehaviour
{
    public GameObject socketHandlerPrefab;//For some reason it only works with prefab

    private void Awake()
    {
        if (FindObjectOfType<SocketHandler>() == null)
        {
            GameObject gm = Instantiate(socketHandlerPrefab);
            DontDestroyOnLoad(gm);
        };
    }
}
