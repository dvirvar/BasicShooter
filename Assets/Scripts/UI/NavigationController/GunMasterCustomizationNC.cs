using UnityEngine;

public class GunMasterCustomizationNC : NavigationController
{
    [SerializeField] private MainMenuCamerasManager mainMenuCamerasManager;

    private void OnEnable()
    {
        mainMenuCamerasManager.moveTo(MainMenuCamerasManager.MainMenuCameraID.customization);
    }

    private void OnDisable()
    {
        mainMenuCamerasManager.moveTo(MainMenuCamerasManager.MainMenuCameraID.main);
    }
}
