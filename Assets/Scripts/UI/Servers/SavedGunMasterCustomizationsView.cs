using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class SavedGunMasterCustomizationsView : MonoBehaviour
{
    public event Action<GunMasterWeapons> usePressed = delegate { };
    public event Action<GunMasterWeapons> deletePressed = delegate { };
    [SerializeField] private Canvas canvas;
    [SerializeField] private SavedGunMasterCustomizationsTableView savedGunMasterCustomizationsTableView;
    [SerializeField] private GameObject savedGunMasterCustomizationsPopupPrefab;
    private List<GunMasterWeapons> gunMasterWeapons;
    private List<SavedGunMasterCustomizationsDisplay> savedGunMasterCustomizationsDisplays;
    private SavedGunMasterCustomizationsPopup currentPopup;

    private void Awake()
    {
        gunMasterWeapons = new List<GunMasterWeapons>();
    }

    private void OnDisable()
    {
        destroyPopupIfPresent();
    }

    private void SavedGunMasterCustomizationsDisplay_OnClicks(SavedGunMasterCustomizationsDisplay savedGunMasterCustomizationsDisplay, PointerEventData.InputButton type, int clickCount)
    {
        if (type == PointerEventData.InputButton.Right)
        {
            switch (clickCount)
            {
                case 1:
                    GameObject gm = Instantiate(savedGunMasterCustomizationsPopupPrefab,transform);
                    Vector2 gmSize = gm.GetComponent<RectTransform>().sizeDelta * canvas.scaleFactor;
                    Vector3 newPos = new Vector3(Input.mousePosition.x + gmSize.x/2, Input.mousePosition.y + gmSize.y/2, Input.mousePosition.z);
                    gm.transform.position = newPos;
                    currentPopup = gm.GetComponent<SavedGunMasterCustomizationsPopup>();
                    currentPopup.setSavedGunMasterCustomizationsDisplay(savedGunMasterCustomizationsDisplay);
                    currentPopup.usePressed += Popup_usePressed;
                    currentPopup.deletePressed += Popup_deletePressed;
                    currentPopup.onDestroy += Popup_onDestroy;
                    break;
                default:
                    break;
            }
        }
    }
    
    private void Popup_usePressed(GunMasterWeapons gunMasterWeapons)
    {
        usePressed(gunMasterWeapons);
    }

    private void Popup_deletePressed(SavedGunMasterCustomizationsDisplay display)
    {
        savedGunMasterCustomizationsTableView.removeInfo(display.info);
        unsubscribeFrom(display);
        deletePressed(display.info);
    }

    private void Popup_onDestroy(SavedGunMasterCustomizationsPopup popup)
    {
        popup.usePressed -= Popup_usePressed;
        popup.deletePressed -= Popup_deletePressed;
        popup.onDestroy -= Popup_onDestroy;
    }

    public void setGunMasterWeapons(List<GunMasterWeapons> gunMasterWeapons)
    {
        if (savedGunMasterCustomizationsDisplays != null)
        {
            unsubscribeFromSavedGunMasterCustomizationsDisplays();
        }
        this.gunMasterWeapons = gunMasterWeapons;
        savedGunMasterCustomizationsTableView.setInfos(gunMasterWeapons);
        savedGunMasterCustomizationsDisplays = savedGunMasterCustomizationsTableView.getDisplays();
        subscribeToSavedGunMasterCustomizationsDisplays();
    }

    private void destroyPopupIfPresent()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup.gameObject);
        }
    }

    public SavedGunMasterCustomizationsTableView getGunMasterCustomizationsTableView()
    {
        return savedGunMasterCustomizationsTableView;
    }

    private void subscribeToSavedGunMasterCustomizationsDisplays()
    {
        foreach (var savedGunMasterCustomizationsDisplay in savedGunMasterCustomizationsDisplays)
        {
            subscribeTo(savedGunMasterCustomizationsDisplay);
        }
    }

    private void unsubscribeFromSavedGunMasterCustomizationsDisplays()
    {
        foreach (var savedGunMasterCustomizationsDisplay in savedGunMasterCustomizationsDisplays)
        {
            unsubscribeFrom(savedGunMasterCustomizationsDisplay);
        }
    }

    private void subscribeTo(SavedGunMasterCustomizationsDisplay savedGunMasterCustomizationsDisplay)
    {
        savedGunMasterCustomizationsDisplay.OnClicks += SavedGunMasterCustomizationsDisplay_OnClicks;
    }

    private void unsubscribeFrom(SavedGunMasterCustomizationsDisplay savedGunMasterCustomizationsDisplay)
    {
        savedGunMasterCustomizationsDisplay.OnClicks -= SavedGunMasterCustomizationsDisplay_OnClicks;
    }
}
