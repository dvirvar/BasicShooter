using System.Collections;
using UnityEngine;
using ExtensionMethods;

public class KillsFeedPanel : PooledTableView<KillFeedInfo, KillFeedInfoDisplay>
{
    [SerializeField] private int maxRows = 5;
    [SerializeField] private float timeToHide = 3;
    protected override PoolObjectID poolObjectID => PoolObjectID.KillFeedDisplay;
    private float hideTimer;
    private IEnumerator currentAnimation;
    private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        hideTimer += Time.deltaTime;
        if (hideTimer >= timeToHide)
        {
            currentAnimation = canvasGroup.animateAlpha(0, 1, () =>
              {
                  currentAnimation = null;
              });
            StartCoroutine(currentAnimation);
            hideTimer = 0;
        }
    }

    public void addKillFeed(string killer, string dead, string weapon)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        canvasGroup.alpha = 1;
        hideTimer = 0;
        var killFeed = new KillFeedInfo()
        {
            killer = killer,
            victim = dead,
            weapon = weapon
        };
        addInfo(killFeed);
        if (getDisplays().Count > maxRows)
        {
            removeInfoAt(maxRows);
        }
    }
}
