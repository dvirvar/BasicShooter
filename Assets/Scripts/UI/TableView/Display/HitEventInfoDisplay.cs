using UnityEngine.UI;
using UnityEngine;
using System;

public class HitEventInfoDisplay : PooledDisplay<HitEventInfo>
{
    public event Action<HitEventInfoDisplay> OnNeedToMoveToPool = delegate { };
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Text rewardText;
    
    public override PoolObjectID id => PoolObjectID.HitEventDisplay;
    private float timeElapsed;
    private readonly float timeToReturn = 3;

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeToReturn)
        {
            OnNeedToMoveToPool(this);
        }
    }

    public override void onDequeue()
    {
        
    }

    public override void onEnqeue(bool isCreated)
    {
        timeElapsed = 0;
    }

    protected override void setView(HitEventInfo info)
    {
        this.previewImage.sprite = info.previewSprite;
        this.descriptionText.text = info.description;
        this.rewardText.text = info.reward.ToString();
        timeElapsed = 0;
    }
}
