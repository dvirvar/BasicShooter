
/// <summary>
/// Listens to Deathmatch network logic
/// </summary>
public class NetworkDeathmatch : NetworkGameMode
{

    private NetworkObjectsManager networkObjectsManager;
    private ScoreboardManager scoreboardManager;
    protected override void Awake()
    {
        base.Awake();
        networkObjectsManager = FindObjectOfType<NetworkObjectsManager>();
        scoreboardManager = FindObjectOfType<ScoreboardManager>();
    }

    private void Start()
    {
        SocketHandler.OnGotDamage += SocketHandler_OnGotDamage;
    }

    private void SocketHandler_OnGotDamage(JSONObject obj)
    {
        var shooter = networkObjectsManager.getPlayer(obj["shooter"].str);
        var victim = networkObjectsManager.getPlayer(obj["victim"].str);
        var points = (int)obj["damage"].n;
        if (shooter.teamID == victim.teamID)
        {
            points *= -1;
        }
        
        scoreboardManager.addGameModePoints(shooter.id, points);
    }

    private void OnDestroy()
    {
        SocketHandler.OnGotDamage -= SocketHandler_OnGotDamage;
    }
}
