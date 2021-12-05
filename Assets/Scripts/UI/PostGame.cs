using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PostGame : MonoBehaviour
{
    [SerializeField] private EditServerNC editServerNC;
    [SerializeField] private EditServerInfoVC editServerInfoVC;
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private Button editServerBtn;
    [SerializeField] private Text countdownText;
    private NetworkObjectsManager networkObjectsManager;
    private string hostID;

    private void Awake()
    {
        networkObjectsManager = FindObjectOfType<NetworkObjectsManager>();
    }

    private void Start()
    {
        editServerBtn.onClick.AddListener(delegate
        {
            editServerNC.SetActive(true);
            scoreboard.SetActive(false);
        });
        editServerNC.willDismiss += EditServerNC_willDismiss;
    }

    private void EditServerNC_willDismiss()
    {
        editServerNC.SetActive(false);
        scoreboard.SetActive(true);
    }

    public void init(WorldState worldState, int? timeout)
    {
        try
        {
            hostID = worldState.serverInfo.hostID;
            bool isSelf = hostID.Equals(User.currentUser().id);
            editServerBtn.gameObject.SetActive(isSelf);
            if (isSelf)
            {
                editServerInfoVC.init(worldState);
            }
            if (timeout.HasValue)
            {
                StartCoroutine(countdown(timeout.Value));
            }
            else
            {
                countdownText.text = "Game will start soon";
            }
        } catch(System.Exception e)
        {
            print(e);
        }
        
    }

    private IEnumerator countdown(int timeout)
    {
        while (timeout > 0)
        {
            if (networkObjectsManager.getPlayer(hostID) == null)
            {
                countdownText.text = $"The host left the server, Server will be closed in: {timeout}";
            } else
            {
                countdownText.text = $"Game will start in: {timeout}";
            }
            yield return new WaitForSeconds(1);
            --timeout;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        editServerBtn.onClick.RemoveAllListeners();
        editServerNC.willDismiss -= EditServerNC_willDismiss;
    }

}
