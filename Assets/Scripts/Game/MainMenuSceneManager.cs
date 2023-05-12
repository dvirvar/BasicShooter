using UnityEngine;
using UnityEngine.UI;

public class MainMenuSceneManager : MonoBehaviour
{
    [SerializeField] private Text pingText;
    private PingSocketService pingSocketService;
    private void Awake()
    {
        pingSocketService = new PingSocketService(FindObjectOfType<SocketHandler>());
    }
    // Start is called before the first frame update
    private void Start()
    {
        FindObjectOfType<AudioHandler>().startBgAudio();
        getLatency();
    }

    private void getLatency()
    {
        pingSocketService.latencySelf(delegate (LatencySelfResponse response)
        {
            try
            {
                if (response.parsedResponse.permission)
                {
                    pingText.text = $"Ping: {response.parsedResponse.latency}";
                }
                Invoke("getLatency", 10);
            }catch (MissingReferenceException)
            {
                CancelInvoke();
            }
        });
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
