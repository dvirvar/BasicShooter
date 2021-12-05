using UnityEngine;
using System;

public class WeaponPartHolder : MonoBehaviour
{
    public event Action<WeaponPartType, Transform> onCustomizationButtonPressed = delegate { };
    [SerializeField] private WeaponPartType type;
    [SerializeField] private WeaponPartLocation customizationLocation;
    private GameObject part;

    // Start is called before the first frame update
    void Start()
    {
        customizationLocation.onButtonPressed += CustomizationLocation_onButtonPressed;
    }

    private void OnDestroy()
    {
        customizationLocation.onButtonPressed -= CustomizationLocation_onButtonPressed;
    }

    private void CustomizationLocation_onButtonPressed(Transform transform)
    {
        onCustomizationButtonPressed(type, transform);
    }

    public void instantiatePart(GameObject part)
    {
        this.part = Instantiate(part, transform, false);
    }

    public void clear()
    {
        if (part != null)
        {
            Destroy(part);
            part = null;
        }
    }

    public void hideWeaponPartLocation()
    {
        customizationLocation.gameObject.SetActive(false);
    }

    public void showWeaponPartLocation()
    {
        customizationLocation.gameObject.SetActive(true);
    }

    public WeaponPartType getWeaponPartType()
    {
        return type;
    }
    
}
