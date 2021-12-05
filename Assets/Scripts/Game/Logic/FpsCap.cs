using UnityEngine;

public class FpsCap : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
