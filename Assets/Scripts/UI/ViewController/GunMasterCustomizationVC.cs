using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections;
using ExtensionMethods;

public class GunMasterCustomizationVC : ViewController
{
    [SerializeField] private Text titleText;
    [SerializeField] private WeaponsListView weaponsListView;
    [SerializeField] private ChosenGunMasterWeaponsView chosenGunMasterWeaponsView;
    [SerializeField] private WeaponStatsView weaponsStatsView;
    [SerializeField] private SavedGunMasterCustomizationsView savedGunMasterCustomizationsView;
    [SerializeField] private GunMasterCustomizationSaveView gunMasterCustomizationSaveView;
    [SerializeField] private WeaponCustomizationSystem weaponCustomizationSystem;
    [SerializeField] private WeaponsPrefabs weaponsPrefabs;
    [SerializeField] private DefaultGunMasterWeapons defaultWeapons;
    [SerializeField] private Popup popup;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private CanvasGroup thisCanvasGroup;
    [SerializeField] private Button customizeBtn;
    
    private List<WeaponListDisplay> weaponListDisplays;
    private GunMasterWeapons currentGunMasterWeapons;

    private void OnEnable()
    {
        savedGunMasterCustomizationsView.usePressed += SavedGunMasterCustomizationsView_usePressed;
        savedGunMasterCustomizationsView.deletePressed += SavedGunMasterCustomizationsView_deletePressed;
        gunMasterCustomizationSaveView.savePressed += GunMasterCustomizationSaveView_savePressed;
    }

    private void OnDisable()
    {
        unsubscribeFromWeaponListDisplays();
        savedGunMasterCustomizationsView.usePressed -= SavedGunMasterCustomizationsView_usePressed;
        savedGunMasterCustomizationsView.deletePressed -= SavedGunMasterCustomizationsView_deletePressed;
        gunMasterCustomizationSaveView.savePressed -= GunMasterCustomizationSaveView_savePressed;
    }

    private void SavedGunMasterCustomizationsView_usePressed(GunMasterWeapons obj)
    {
        setCurrentGunMasterWeapons(obj);
    }

    private void SavedGunMasterCustomizationsView_deletePressed(GunMasterWeapons obj)
    {
        UserPrefs.deleteGunMasterWeapons(obj);
    }

    private void GunMasterCustomizationSaveView_savePressed(string name)
    {
        var validation = isGunMasterWeaponsValid(name);
        if (!validation.valid)
        {
            popup.showWith(validation.reason);
            return;
        }
        GunMasterWeapons gunMasterWeapons = new GunMasterWeapons();
        gunMasterWeapons.name = name;
        gunMasterWeapons.weapons = chosenGunMasterWeaponsView.getWeaponListDisplays().ConvertAll(contertToWeaponInfo).ToArray();
        UserPrefs.addGunMasterWeapons(gunMasterWeapons);
        setCurrentGunMasterWeapons(gunMasterWeapons);
        UserPrefs.setGunMasterWeaponsLastSelected(name);
    }

    private (bool valid, string reason) isGunMasterWeaponsValid(string name)
    {
        //TODO: Static for min weapons in Gun Master
        if (chosenGunMasterWeaponsView.getWeaponListDisplays().Count < 2)
        {
            return (false, "Choose at least 2 weapons");
        } else if (name == ""){
            return (false, "Choose a name for your customization");
        } else if (defaultWeapons.contains(name))
        {
            return (false, "Can't save with this name");
        }

        return (true, "");
    }

    public override void backButtonPressed()
    {
        weaponsListView.removeAllWeaponListDisplays();
        chosenGunMasterWeaponsView.removeAllWeaponListDisplays();
        base.backButtonPressed();
    }

    public void customizeBtnPressed()
    {
        StartCoroutine(startCustomization());
    }

    private IEnumerator startCustomization()
    {
        bool isThisCanvasGroupAlphaIsZero()
        {
            return thisCanvasGroup.alpha == 0;
        }
        StartCoroutine(thisCanvasGroup.animateAlpha(0, 0.5f));
        yield return new WaitUntil(isThisCanvasGroupAlphaIsZero);
        weaponCustomizationSystem.OnBack += WeaponCustomizationSystem_OnBack;
        weaponCustomizationSystem.startCustomization();
        thisCanvasGroup.blocksRaycasts = false;
    }

    private void WeaponCustomizationSystem_OnBack()
    {
        StartCoroutine(thisCanvasGroup.animateAlpha(1, 0.5f));
        this.thisCanvasGroup.blocksRaycasts = true;
        weaponCustomizationSystem.OnBack -= WeaponCustomizationSystem_OnBack;
    }

    public void saveAndExit()
    {
        string name = gunMasterCustomizationSaveView.getCustomizationName();
        var validation = isGunMasterWeaponsValid(name);
        if (!validation.valid)
        {
            popup.showWith(validation.reason);
            return;
        }
        GunMasterWeapons gunMasterWeapons = new GunMasterWeapons();
        gunMasterWeapons.name = name;
        gunMasterWeapons.weapons = chosenGunMasterWeaponsView.getWeaponListDisplays().ConvertAll(contertToWeaponInfo).ToArray();
        UserPrefs.addGunMasterWeapons(gunMasterWeapons);
        UserPrefs.setGunMasterWeaponsLastSelected(name);
        backButtonPressed();
    }

    public void resetView()
    {
        setCurrentGunMasterWeapons(null);
    }

    public void setCurrentGunMasterWeapons(GunMasterWeapons currentGunMasterWeapons)
    {
        if (weaponListDisplays != null)
        {
            unsubscribeFromWeaponListDisplays();
            weaponsListView.removeAllWeaponListDisplays();
            chosenGunMasterWeaponsView.removeAllWeaponListDisplays();
            weaponListDisplays.Clear();            
        }
        this.currentGunMasterWeapons = currentGunMasterWeapons;
        initializeView();
        weaponsStatsView.clear();
    }

    private void initializeView()
    {
        initializeWeaponsListItemAndChosenWeaponsListItem();
        initializeSavedCustomizationsView();
        initializeGunMasterCustomizationSaveView();
        initializeWeaponCustomizationSystem();
    }

    private void initializeWeaponCustomizationSystem()
    {
        weaponCustomizationSystem.clear();
        customizeBtn.gameObject.SetActive(false);
    }

    private void initializeGunMasterCustomizationSaveView()
    {
        if (currentGunMasterWeapons == null)
        {
            gunMasterCustomizationSaveView.setName("");
        } else
        {
            gunMasterCustomizationSaveView.setName(currentGunMasterWeapons.name);
        }        
    }

    private void initializeSavedCustomizationsView()
    {
        savedGunMasterCustomizationsView.setGunMasterWeapons(UserPrefs.getGunMasterWeapons());
    }

    private void initializeWeaponsListItemAndChosenWeaponsListItem()
    {
        List<CustomizableWeapon> allCustomizableWeapons = weaponsPrefabs.getCopyOfCustomizableWeapons();
        List<WeaponListItem> allWeapons = allCustomizableWeapons.ConvertAll(convertToWeaponListItem);
        weaponsListView.setWeaponListItems(allWeapons);
        weaponListDisplays = weaponsListView.getWeaponListDisplays();
        subscribeToWeaponListDisplays();
        if (currentGunMasterWeapons == null)
        {
            titleText.text = "Gun Master Customization";
        }
        else
        {
            titleText.text = currentGunMasterWeapons.name;
            foreach (var weaponInfo in currentGunMasterWeapons.weapons)
            {
                WeaponListDisplay weaponListDisplay = weaponListDisplays.Find(wld => wld.info.weaponInfo.Equals(weaponInfo));
                if (weaponListDisplay != null)
                {
                    weaponListDisplay.info.setWeaponInfo(weaponInfo);
                    moveWeaponListDisplayToChosenView(weaponListDisplay);
                }
            }
        }
    }

    private WeaponListItem convertToWeaponListItem(CustomizableWeapon customizableWeapon)
    {
        return new WeaponListItem(customizableWeapon);
    }

    private WeaponInfo contertToWeaponInfo(WeaponListDisplay weaponListDisplay)
    {
        return weaponListDisplay.info.weaponInfo;
    }

    private void WeaponListDisplay_OnHover(WeaponStats weaponStats)
    {
        weaponsStatsView.compareValues(weaponStats);
    }

    private void WeaponListDisplay_OnStoppedHovering()
    {
        weaponsStatsView.stopComparingValues();
    }

    private void WeaponListDisplay_OnClicks(WeaponListDisplay weaponListDisplay, PointerEventData.InputButton type, int clickCount)
    {
        if(type == PointerEventData.InputButton.Left)
        {
            switch (clickCount)
            {
                case 1:
                    customizeBtn.gameObject.SetActive(true);
                    weaponCustomizationSystem.setWeapon(weaponListDisplay.info.weaponInfo);
                    weaponCustomizationSystem.hidePartLocations();
                    weaponsStatsView.setValues(weaponListDisplay.info.weaponStats);
                    break;
                case 2:
                    if (weaponListDisplay.chosenState)
                    {
                        weaponListDisplay.clearChosenGunMasterWeaponDisplay();
                        moveWeaponListDisplayToWeaponListView(weaponListDisplay, 0);
                    } else
                    {
                        moveWeaponListDisplayToChosenView(weaponListDisplay);
                    }
                    break;
                default:
                    break;
            }
        } else if(type == PointerEventData.InputButton.Right)
        {
            switch (clickCount)
            {
                case 1:
                    if (weaponListDisplay.chosenState)
                    {
                        weaponListDisplay.clearChosenGunMasterWeaponDisplay();
                        moveWeaponListDisplayToWeaponListView(weaponListDisplay, 0);
                    } else
                    {
                        moveWeaponListDisplayToChosenView(weaponListDisplay);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void WeaponListDisplay_OnBeganHolding(WeaponListDisplay weaponListDisplay)
    {
        if (!weaponListDisplay.chosenState)
        {
            weaponListDisplay.shrinkAnimation();
        }
        weaponListDisplay.transform.SetParent(transform, true);
    }

    private void WeaponListDisplay_OnStoppedHolding(WeaponListDisplay weaponListDisplay, Transform parentTransform, int originalIndex, PointerEventData eventData)
    {
        List<RaycastResult> rr = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, rr);
        foreach (var item in rr)
        {
            if (item.gameObject.TryGetComponent(out ChosenGunMasterWeaponDisplay comp))
            {
                bool cameFromChosenView = weaponListDisplay.chosenState;
                ChosenGunMasterWeaponDisplay chosenGunMasterWeaponDisplay = weaponListDisplay.getChosenGunMasterWeaponDisplay();
                WeaponListDisplay previous = comp.setWeaponListDisplay(weaponListDisplay);
                if (previous != null)
                {
                    if (cameFromChosenView)
                    {
                        chosenGunMasterWeaponDisplay.setWeaponListDisplay(previous);
                    } else
                    {
                        moveWeaponListDisplayToWeaponListView(previous, originalIndex);
                    }
                } else
                {
                    if (cameFromChosenView)
                    {
                        chosenGunMasterWeaponDisplay.clear();
                    }
                }
                return;
            }
        }
        //We didnt stop on ChosenGunMasterWeaponDisplay
        //Move it to WeaponListView
        moveWeaponListDisplayToWeaponListView(weaponListDisplay, originalIndex);
    }

    private void moveWeaponListDisplayToChosenView(WeaponListDisplay weaponListDisplay)
    {
        weaponsListView.removeWeaponListDisplay(weaponListDisplay);
        if (!chosenGunMasterWeaponsView.setWeaponListDisplay(weaponListDisplay))
        {
            popup.showWith("All slots are full");
        }
    }

    private void moveWeaponListDisplayToWeaponListView(WeaponListDisplay weaponListDisplay, int originalIndex)
    {
        weaponsListView.setWeaponListDisplay(weaponListDisplay, originalIndex);
    }

    private void subscribeToWeaponListDisplays()
    {
        foreach (var weaponListDisplay in weaponListDisplays)
        {
            subscribeTo(weaponListDisplay);
        }
    }

    private void unsubscribeFromWeaponListDisplays()
    {
        foreach (var weaponListDisplay in weaponListDisplays)
        {
            unsubscribeFrom(weaponListDisplay);
        }
    }

    private void subscribeTo(WeaponListDisplay weaponListDisplay)
    {
        weaponListDisplay.OnBeganHovering += WeaponListDisplay_OnHover;
        weaponListDisplay.OnStoppedHovering += WeaponListDisplay_OnStoppedHovering;
        weaponListDisplay.OnClicks += WeaponListDisplay_OnClicks;
        weaponListDisplay.OnMouseButtonUp += WeaponListDisplay_OnMouseButtonUp;
        weaponListDisplay.OnBeganHolding += WeaponListDisplay_OnBeganHolding;
        weaponListDisplay.OnStoppedHolding += WeaponListDisplay_OnStoppedHolding;
    }

    private void WeaponListDisplay_OnMouseButtonUp(WeaponListDisplay obj, bool isHolding)
    {
        if (obj.chosenState && !isHolding)
        {
            obj.transform.localPosition = Vector3.zero;
        }
    }

    private void unsubscribeFrom(WeaponListDisplay weaponListDisplay)
    {
        weaponListDisplay.OnBeganHovering -= WeaponListDisplay_OnHover;
        weaponListDisplay.OnStoppedHovering -= WeaponListDisplay_OnStoppedHovering;
        weaponListDisplay.OnClicks -= WeaponListDisplay_OnClicks;
        weaponListDisplay.OnMouseButtonUp -= WeaponListDisplay_OnMouseButtonUp;
        weaponListDisplay.OnBeganHolding -= WeaponListDisplay_OnBeganHolding;
        weaponListDisplay.OnStoppedHolding -= WeaponListDisplay_OnStoppedHolding;
    }
}
