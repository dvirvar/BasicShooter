using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ExtensionMethods;
/// <summary>
/// Handles scene in a higher level for example:
/// Listening to on socket close and showing a popup about reconnecting
/// Also trying to reconnect and decide which scene to go
/// </summary>
public class AppSceneManager : MonoBehaviour
{
    private static AppSceneManager instance;
    private SocketHandler socketHandler;
    private int retryInterval = 5;
    private int retryTimesTimeout = 3;
    [SerializeField] private Popup popupPrefab;
    private Popup popup;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (instance == null)
        {
            instance = new GameObject("AppSceneManager").AddComponent<AppSceneManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    void Start() {
        socketHandler = FindObjectOfType<SocketHandler>();
        SocketHandler.OnSocketClose += NetworkHandler_OnSocketClose;
    }

    private void NetworkHandler_OnSocketClose(ushort code)
    {
        if (code == 1005)
        {
            return;
        }
        Dispatcher.RunOnMainThread(delegate
        {
            popup = Instantiate(popupPrefab, FindObjectOfType<Canvas>().transform);
            popup.GetComponent<RectTransform>().setStreched();
            popup.showCloseBtnOnEnabled = false;
            popup.showWith("Reconnecting...");
            StartCoroutine(reconnectSocket());
        });
    }

    private IEnumerator reconnectSocket()
    {
        for (int i = 0; i < retryTimesTimeout; i++)
        {
            socketHandler.Connect();
            if (socketHandler.IsConnected)
            {
                break;
            }
            yield return new WaitForSeconds(retryInterval);
            if (socketHandler.IsConnected)
            {
                break;
            }
        }
        Destroy(popup.gameObject);
        if (!socketHandler.IsConnected)
        {
            SceneManager.LoadSceneAsync("Credentials");
        } else if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    private void OnDestroy()
    {
        SocketHandler.OnSocketClose -= NetworkHandler_OnSocketClose;
    }
}