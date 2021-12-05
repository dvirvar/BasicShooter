using UnityEngine;

public class ServersFilterBehavior : MonoBehaviour
{
    public GameObject filterSegmentsContainer;
    private FilterSegment[] filterSegments;
    private FilterSegment currentOpen;
    // Start is called before the first frame update
    void Start()
    {
        filterSegments = filterSegmentsContainer.GetComponentsInChildren<FilterSegment>();
        for (int i = 0; i < filterSegments.Length; i++)
        {
            filterSegments[i].onClick += onClickSegment;
        }
    }
    private void onClickSegment(FilterSegment filterSegment)
    {
        if (currentOpen == filterSegment)
        {
            currentOpen.setWindowActive(!currentOpen.isWindowClosed);
            return;
        }
        if (currentOpen != null)
        {
            currentOpen.setWindowActive(false);
        }
        currentOpen = filterSegment;
        filterSegment.setWindowActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnDisabled()
    {
        foreach (FilterSegment filterSegment in filterSegments)
        {
            filterSegment.setWindowActive(false);
        }
    }
    void OnDestroy()
    {
        foreach (FilterSegment filterSegment in filterSegments)
        {
            filterSegment.onClick -= onClickSegment;
        }
    }
}
