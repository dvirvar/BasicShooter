using UnityEngine;

public class MainMenuCamerasManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera customizationCamera;

    public void moveTo(MainMenuCameraID cameraID)
    {
        switch (cameraID)
        {
            case MainMenuCameraID.main:
                mainCamera.gameObject.SetActive(true);
                customizationCamera.gameObject.SetActive(false);
                break;
            case MainMenuCameraID.customization:
                mainCamera.gameObject.SetActive(false);
                customizationCamera.gameObject.SetActive(true);
                break;
        }
    }

    public enum MainMenuCameraID
    {
        main,
        customization
    }
}
