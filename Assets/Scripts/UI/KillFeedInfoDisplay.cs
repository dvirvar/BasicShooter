using UnityEngine.UI;
using UnityEngine;

public class KillFeedInfoDisplay : PooledDisplay<KillFeedInfo>
{
    [SerializeField]
    private Text killer;
    [SerializeField]
    private Text weapon;
    [SerializeField]
    private Text victim;

    public override PoolObjectID id => PoolObjectID.KillFeedDisplay;

    public override void onDequeue()
    {
        
    }

    public override void onEnqeue(bool isCreated)
    {
        
    }

    protected override void setView(KillFeedInfo info)
    {
        this.killer.text = info.killer;
        this.weapon.text = $"[{info.weapon}]";
        this.victim.text = info.victim;
    }
}
