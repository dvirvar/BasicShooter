using UnityEngine;
using System.Linq;
using System;

public class WeaponCustomizationSystem : MonoBehaviour
{
    public event Action OnBack = delegate { };
    [SerializeField] private Canvas customizationCanvas;
    [SerializeField] private Camera customizationCamera;
    [SerializeField] private WeaponsPrefabs weaponsPrefabs;
    [SerializeField] private WeaponPartsPrefabs weaponPartsPrefabs;
    [SerializeField] private WeaponPartsListView weaponPartsListView;
    [SerializeField] private GameObject weaponHolder;
    private ObjectMover objectMover;
    private ObjectRotator objectRotator;
    private GameObject currentWeaponObj;
    private WeaponPartsBuilder currentWeaponPartsBuilder;
    private WeaponPartType? currentWeaponPartType;
    private Vector3 initialRotation;

    private void Awake()
    {
        objectMover = customizationCamera.GetComponent<ObjectMover>();
        objectRotator = weaponHolder.GetComponent<ObjectRotator>();
    }

    private void Start()
    {
        initialRotation = weaponHolder.transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            stopCustomization();
            OnBack();
        }
    }

    public void setWeapon(WeaponInfo weaponInfo)
    {
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
            stopListening();
        }
        currentWeaponObj = Instantiate(weaponsPrefabs.getCustomizablePrefab(weaponInfo.getID()));
        currentWeaponObj.transform.SetParent(weaponHolder.transform);
        currentWeaponObj.transform.localPosition = Vector3.zero;
        currentWeaponObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        currentWeaponPartsBuilder = currentWeaponObj.GetComponent<WeaponPartsBuilder>();
        currentWeaponPartsBuilder.setWeaponInfo(weaponInfo);
        
    }

    public void hidePartLocations()
    {
        currentWeaponPartsBuilder.hidePartsLocations();
    }

    public void showPartLocations()
    {
        currentWeaponPartsBuilder.showPartsLocations();
    }

    public void stopListening()
    {
        if (currentWeaponPartsBuilder != null)
        {
            currentWeaponPartsBuilder.onPartLocationPresssed -= CurrentWeaponPartsBuilder_onPartLocationPresssed;
        }
    }

    public void startListening()
    {
        if (currentWeaponPartsBuilder != null)
        {
            currentWeaponPartsBuilder.onPartLocationPresssed += CurrentWeaponPartsBuilder_onPartLocationPresssed;
        }        
    }

    public void stopCustomization()
    {
        customizationCanvas.gameObject.SetActive(false);
        hidePartLocations();
        stopListening();
        enableScriptsMovers(false);
        weaponPartsListView.clear();
        currentWeaponPartType = null;
        weaponHolder.transform.eulerAngles = initialRotation;
    }

    public void startCustomization()
    {
        customizationCanvas.gameObject.SetActive(true);
        showPartLocations();
        startListening();
        enableScriptsMovers(true);
    }

    private void enableScriptsMovers(bool enable)
    {
        objectMover.enabled = enable;
        objectRotator.enabled = enable;
    }
    
    private void CurrentWeaponPartsBuilder_onPartLocationPresssed(WeaponPartType wpt, Transform transform)
    {
        stopListeningToWeaponPartDisplays();
        if (currentWeaponPartType != wpt)
        {
            currentWeaponPartType = wpt;
            switch (wpt)
            {
                case WeaponPartType.scope:
                    weaponPartsListView.setWeaponPartItems(weaponPartsPrefabs.scopes.Select(sph => new WeaponPartItem(sph)).ToList());
                    break;
            }
        }
        startListeningToWeaponPartDisplays();
    }

    private void WeaponPartDisplay_OnClick(WeaponPartItem wpi)
    {
        switch(currentWeaponPartType)
        {
            case WeaponPartType.scope:
                ScopeID scope = (ScopeID)wpi.weaponPartPrefabHolder.id;
                currentWeaponPartsBuilder.setScope(scope);
                break;
        }
    }

    private void startListeningToWeaponPartDisplays()
    {
        weaponPartsListView.getDisplays().ForEach(weaponPartDisplay =>
        {
            weaponPartDisplay.OnClick += WeaponPartDisplay_OnClick;
        });
    }

    private void stopListeningToWeaponPartDisplays()
    {
        weaponPartsListView.getDisplays().ForEach(weaponPartDisplay =>
        {
            weaponPartDisplay.OnClick -= WeaponPartDisplay_OnClick;
        });
    }

    public void clear()
    {
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
        }
        stopListening();
        currentWeaponPartsBuilder = null;
    }
}
