using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutVC : ViewController
{
    /// <summary>
    /// Represents the current pressed weapon/utility, so when pressing on a weapon/utility on list it will know where to put it
    /// </summary>
    private enum CurrentType
    {
        Primary,Secondary
    }
    [SerializeField] private CanvasGroup thisCanvasGroup;
    [SerializeField] private MainMenuCamerasManager mainMenuCamerasManager;
    [SerializeField] private WeaponsPrefabs weaponsPrefabs;
    [SerializeField] private WeaponCustomizationSystem weaponCustomizationSystem;
    [SerializeField] private Toggle[] loadoutButtons;
    [SerializeField] private CustomizationWeaponUIHolder primaryWeaponHolder;
    [SerializeField] private CustomizationWeaponUIHolder secondaryWeaponHolder;
    [SerializeField] private WeaponsTableView weaponsTableView;
    [SerializeField] private WeaponStatsView weaponStatsView;
    private List<WeaponListItem> primaries;
    private List<WeaponListItem> secondaries;
    private Loadout currentLoadout;
    private int loadoutNumber = -1;
    private CurrentType? currentType;
    private SocketHandler socketHandler;
    private WeaponarySocketService weaponarySocketService;
    private WeaponInfo currentCustomizedWeapon;

    private void Awake()
    {
        loadoutButtons[0].onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                setCurrentLoadout(0);
            }
        });
        loadoutButtons[1].onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                setCurrentLoadout(1);
            }
        });
        loadoutButtons[2].onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                setCurrentLoadout(2);
            }
        });
        primaryWeaponHolder.WeaponPressed += PrimaryWeaponHolder_WeaponPressed;
        secondaryWeaponHolder.WeaponPressed += SecondaryWeaponHolder_WeaponPressed;
        primaryWeaponHolder.CustomizeWeaponPressed += WeaponHolder_CustomizeWeaponPressed;
        secondaryWeaponHolder.CustomizeWeaponPressed += WeaponHolder_CustomizeWeaponPressed;
        weaponsTableView.OnItemClicked += WeaponsTableView_OnItemClicked;
        weaponsTableView.OnItemHover += WeaponsTableView_OnItemHover;
        socketHandler = FindObjectOfType<SocketHandler>();
        weaponarySocketService = new WeaponarySocketService(socketHandler);
    }

    #region delegates
    private void PrimaryWeaponHolder_WeaponPressed()
    {
        if (currentType != CurrentType.Primary || currentType == CurrentType.Primary && !weaponsTableView.gameObject.activeInHierarchy)
        {
            weaponsTableView.gameObject.SetActive(true);
            currentType = CurrentType.Primary;
            weaponsTableView.setInfos(primaries, currentLoadout.getPrimary());
            weaponStatsView.setValues(weaponsPrefabs.getWeaponStats(currentLoadout.getPrimary().getID()));
        } else
        {
            weaponsTableView.gameObject.SetActive(false);
        }
    }

    private void SecondaryWeaponHolder_WeaponPressed()
    {
        if (currentType != CurrentType.Secondary || currentType == CurrentType.Secondary && !weaponsTableView.gameObject.activeInHierarchy)
        {
            weaponsTableView.gameObject.SetActive(true);
            currentType = CurrentType.Secondary;
            weaponsTableView.setInfos(secondaries, currentLoadout.getSecondary());
            weaponStatsView.setValues(weaponsPrefabs.getWeaponStats(currentLoadout.getSecondary().getID()));
        } else
        {
            weaponsTableView.gameObject.SetActive(false);
        }
    }

    private void WeaponHolder_CustomizeWeaponPressed(WeaponInfo weaponInfo)
    {
        currentCustomizedWeapon = weaponInfo;
        enableThisCanvasGroup(false);
        mainMenuCamerasManager.moveTo(MainMenuCamerasManager.MainMenuCameraID.customization);
        weaponCustomizationSystem.setWeapon(weaponInfo);
        weaponCustomizationSystem.OnBack += WeaponCustomizationSystem_OnBack;
        weaponCustomizationSystem.startCustomization();
    }

    private void WeaponCustomizationSystem_OnBack()
    {
        enableThisCanvasGroup(true);
        weaponCustomizationSystem.clear();
        weaponCustomizationSystem.OnBack -= WeaponCustomizationSystem_OnBack;
        mainMenuCamerasManager.moveTo(MainMenuCamerasManager.MainMenuCameraID.main);
        weaponarySocketService.customizeWeapon(currentCustomizedWeapon,delegate(BasicSocketResponse response)
        {
            print(response.ToString());
        });
        currentCustomizedWeapon = null;
    }

    private void enableThisCanvasGroup(bool enable)
    {
        thisCanvasGroup.blocksRaycasts = enable;
        thisCanvasGroup.alpha = enable ? 1 : 0;
    }

    private void WeaponsTableView_OnItemClicked(CustomizableWeapon obj)
    {
        if (!currentLoadout.hasWeapon(obj.weaponInfo))
        {
            weaponsTableView.setChosenWeapon(obj.weaponInfo);
            currentLoadout.setWeaponByType(obj.weaponInfo);
            weaponStatsView.setValues(obj.weaponStats);
            if (currentType == CurrentType.Primary)
            {
                primaryWeaponHolder.setWeaponInfo(obj.weaponInfo);
            } else if (currentType == CurrentType.Secondary)
            {
                secondaryWeaponHolder.setWeaponInfo(obj.weaponInfo);
            }
        }
    }

    private void WeaponsTableView_OnItemHover(bool isHover, CustomizableWeapon obj)
    {
        if (isHover)
        {
            weaponStatsView.compareValues(obj.weaponStats);
        } else
        {
            weaponStatsView.stopComparingValues();
        }
    }
    #endregion

    void Start()
    {
        loadoutButtons[0].onValueChanged.Invoke(true);
        populateLists();
    }

    private void populateLists()
    {
        primaries = new List<WeaponListItem>();
        secondaries = new List<WeaponListItem>();
        List<WeaponListItem> list = weaponsPrefabs.getCopyOfCustomizableWeapons().ConvertAll(convertToWeaponListItem);
        foreach (var wli in list)
        {
            if (wli.weaponInfo.getWeaponType() == WeaponType.Primary)
            {
                primaries.Add(wli);
            } else
            {
                secondaries.Add(wli);
            }
        }
    }

    private WeaponListItem convertToWeaponListItem(CustomizableWeapon customizableWeapon)
    {
        return new WeaponListItem(customizableWeapon);
    }

    private void setCurrentLoadout(int number)
    {
        if (loadoutNumber == number)
        {
            return;
        }
        loadoutNumber = number;
        currentLoadout = User.currentUser().playerLoadouts[number];
        
        primaryWeaponHolder.setWeaponInfo(currentLoadout.getPrimary());
        secondaryWeaponHolder.setWeaponInfo(currentLoadout.getSecondary());
        if (weaponsTableView.gameObject.activeInHierarchy)
        {
            if (currentType == CurrentType.Primary)
            {
                weaponsTableView.setChosenWeapon(currentLoadout.getPrimary());
                weaponStatsView.setValues(weaponsPrefabs.getWeaponStats(currentLoadout.getPrimary().getID()));
            }
            else
            {
                weaponsTableView.setChosenWeapon(currentLoadout.getSecondary());
                weaponStatsView.setValues(weaponsPrefabs.getWeaponStats(currentLoadout.getSecondary().getID()));
            }
        }
    }

    public override void backButtonPressed()
    {
        weaponarySocketService.customizeLoadouts(User.currentUser().playerLoadouts, delegate (BasicSocketResponse response)
         {
             print(response.ToString());
         });
        base.backButtonPressed();
    }

    private void OnDestroy()
    {
        foreach (var button in loadoutButtons)
        {
            button.onValueChanged.RemoveAllListeners();
        }
        primaryWeaponHolder.WeaponPressed -= PrimaryWeaponHolder_WeaponPressed;
        secondaryWeaponHolder.WeaponPressed -= SecondaryWeaponHolder_WeaponPressed;
        primaryWeaponHolder.CustomizeWeaponPressed -= WeaponHolder_CustomizeWeaponPressed;
        secondaryWeaponHolder.CustomizeWeaponPressed -= WeaponHolder_CustomizeWeaponPressed;
        weaponsTableView.OnItemClicked -= WeaponsTableView_OnItemClicked;
        weaponsTableView.OnItemHover -= WeaponsTableView_OnItemHover;
    }
}
