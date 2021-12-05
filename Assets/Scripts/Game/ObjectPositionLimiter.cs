using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectPositionLimiter : MonoBehaviour
{
    [SerializeField] private GameObject gm;
    private new Collider collider;
    private Vector3 lastGmPos;

    private void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        lastGmPos = gm.transform.position;
    }

    private void LateUpdate()
    {
        if (!collider.bounds.Contains(gm.transform.position))
        {
            gm.transform.position = lastGmPos;
        }
    }

}