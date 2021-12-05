using UnityEngine;
using UnityEngine.UI;
using System;

public class SavedGunMasterCustomizationsPopup : MonoBehaviour
{
    public event Action<GunMasterWeapons> usePressed = delegate { };
    public event Action<SavedGunMasterCustomizationsDisplay> deletePressed = delegate { };
    public event Action<SavedGunMasterCustomizationsPopup> onDestroy = delegate { };
    [SerializeField] private Button useBtn;
    [SerializeField] private Button deleteBtn;
    private SavedGunMasterCustomizationsDisplay savedGunMasterCustomizationsDisplay;

    private void Awake()
    {
        useBtn.onClick.AddListener(delegate
        {
            usePressed(savedGunMasterCustomizationsDisplay.info);
            Destroy(gameObject);
        });

        deleteBtn.onClick.AddListener(delegate
        {
            deletePressed(savedGunMasterCustomizationsDisplay);
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        if (Input.GetButtonUp(StaticStrings.Weapon.fire) || Input.GetMouseButtonUp(1)) {
            Destroy(gameObject);
        }
    }

    public void setSavedGunMasterCustomizationsDisplay(SavedGunMasterCustomizationsDisplay savedGunMasterCustomizationsDisplay)
    {
        this.savedGunMasterCustomizationsDisplay = savedGunMasterCustomizationsDisplay;
    }

    private void OnDestroy()
    {
        onDestroy(this);
        useBtn.onClick.RemoveAllListeners();
        deleteBtn.onClick.RemoveAllListeners();
    }

}
