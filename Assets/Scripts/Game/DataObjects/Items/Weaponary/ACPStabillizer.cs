using UnityEngine;
/// <summary>
/// For some reason sometimes the fire transfor's Y is getting stuck in 360 
/// So it will fix it
/// </summary>
public class ACPStabillizer : MonoBehaviour
{
    [SerializeField] private Transform fireTransform;
    private void Update()
    {
        if (fireTransform.localEulerAngles.y != 270)
        {
            fireTransform.localEulerAngles = new Vector3(fireTransform.localEulerAngles.x, 270, fireTransform.localEulerAngles.z);
        }
    }
}
