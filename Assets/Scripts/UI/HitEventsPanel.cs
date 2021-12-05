using UnityEngine;

public class HitEventsPanel : PooledTableView<HitEventInfo,HitEventInfoDisplay>
{
    protected override PoolObjectID poolObjectID => PoolObjectID.HitEventDisplay;
    [SerializeField] private Sprite killEnemySprite, damageSprite;
    private int lastPointsIndex => getDisplays().Count - damageInfos;
    private int damageInfos = 0;

    public void addDamage(string name, int damage)
    {        
        var displayIndex = getIndexOfDisplay(name);     
        if (displayIndex != -1)
        {
            var display = infosDisplay[displayIndex];
            display.info.reward += damage;
            display.refreshDisplay();
            changeIndex(displayIndex, getDisplays().Count - 1);
        }
        else
        {
            var hitEventInfo = new HitEventInfo()
            {
                id = name,
                previewSprite = damageSprite,
                description = $"Hit {name}",
                reward = damage
            };
            addInfo(hitEventInfo);
            ++damageInfos;
        }
    }

    public void addDeath(string name, int points)
    {
        var hitEventInfo = new HitEventInfo()
        {
            id = $"{name}dead",
            previewSprite = killEnemySprite,
            description = $"Killed {name}",
            reward = points,
            isDeadPoints = true
        };
        var index = damageInfos > 4 ? getDisplays().Count - 4 : lastPointsIndex;
        var display = addInfo(hitEventInfo, index);
        display.OnNeedToMoveToPool += OnNeedToMoveToPool;
    }
    private int getIndexOfDisplay(string id)
    {
        for (int i = 0; i < infosDisplay.Count; i++)
        {
            if (infosDisplay[i].info.id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }

    public override void addInfo(HitEventInfo info)
    {
        var display = createAndFillInfoDisplay(info);
        infosDisplay.Add(display);
        display.OnNeedToMoveToPool += OnNeedToMoveToPool;
    }

    private void OnNeedToMoveToPool(HitEventInfoDisplay display)
    {
        if (!display.info.isDeadPoints)
        {
            --damageInfos;
        }
        display.OnNeedToMoveToPool -= OnNeedToMoveToPool;
        removeInfoAt(display.transform.GetSiblingIndex());
    }

    public override void removeAll()
    {
        for (int i = infosDisplay.Count - 1; i >= 0; i--)
        {
            OnNeedToMoveToPool(infosDisplay[i]);
        }
        infosDisplay.Clear();
    }
}
