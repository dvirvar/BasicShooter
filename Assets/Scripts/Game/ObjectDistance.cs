using UnityEngine;

public class ObjectDistance : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    // Update is called once per frame
    void Update()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0)
        {
            Vector3 nextPos = transform.position + transform.forward * mouseWheel * Time.deltaTime * 20;
            float zoomOffset = Vector3.Distance(nextPos, target.transform.position);
            if (zoomOffset > minDistance && zoomOffset < maxDistance)
            {
                transform.position = nextPos;
            }
        }
    }
}
