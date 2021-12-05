using UnityEngine;
using System;
/// <summary>
/// Handles the logic of the player loadout
/// </summary>
public class LoadoutHolder : MonoBehaviour
{
    public event Action<Weapon> onWeaponChanged = delegate { };
    
    [SerializeField] private GameObject primaryHolder;
    [SerializeField] private GameObject secondaryHolder;
    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    private Weapon _currentActiveWeapon;
    public Weapon currentActiveWeapon
    {
        get => _currentActiveWeapon;
        private set
        {
            if (value != null)
            {
                bool isPrimary = value.getWeaponInfo().getWeaponType() == WeaponType.Primary;
                primaryHolder.SetActive(isPrimary);
                secondaryHolder.SetActive(!isPrimary);
            } else
            {
                primaryHolder.SetActive(false);
                secondaryHolder.SetActive(false);
                if (_currentActiveWeapon == primaryWeapon)
                {
                    primaryWeapon = null;
                } else if (_currentActiveWeapon == secondaryWeapon)
                {
                    secondaryWeapon = null;
                }
            }
            _currentActiveWeapon = value;
            onWeaponChanged(value);
        }
    }

    public void init(Weapon primary, Weapon secondary)
    {
        if (secondaryWeapon != null)
        {
            Destroy(secondaryWeapon.gameObject);
            if (secondaryWeapon == currentActiveWeapon)
            {
                currentActiveWeapon = null;
            }
        }
        if (primaryWeapon != null)
        {
            Destroy(primaryWeapon.gameObject);
            if (primaryWeapon == currentActiveWeapon)
            {
                currentActiveWeapon = null;
            }
        }
        if (secondary != null)
        {
            setWeapon(secondary);

        }
        if (primary != null)
        {
            setWeapon(primary);
        }
    }

    public bool switchWeapon()
    {
        if (primaryHolder.activeInHierarchy)
        {
            if (secondaryWeapon != null)
            {
                currentActiveWeapon?.resetState();
                currentActiveWeapon = secondaryWeapon;
                return true;
            }
            return false;
        }
        if (secondaryHolder.activeInHierarchy)
        {
            if (primaryWeapon != null)
            {
                currentActiveWeapon?.resetState();
                currentActiveWeapon = primaryWeapon;
                return true;
            }
        }
        return false;
    }

    public bool switchWeapon(int index)
    {
        switch (index)
        {
            case 0:
                if (primaryWeapon != null && primaryWeapon != currentActiveWeapon)
                {
                    currentActiveWeapon?.resetState();
                    currentActiveWeapon = primaryWeapon;
                    return true;
                }
                break;
            case 1:
                if (secondaryWeapon != null && secondaryWeapon != currentActiveWeapon)
                {
                    currentActiveWeapon?.resetState();
                    currentActiveWeapon = secondaryWeapon;
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    private void setWeapon(Weapon weapon)
    {
        if (weapon.getWeaponInfo().getWeaponType() == WeaponType.Primary)
        {
            primaryWeapon = weapon;
            primaryWeapon.transform.SetParent(primaryHolder.transform);
            primaryWeapon.transform.localPosition = Vector3.zero;
            primaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        } else if (weapon.getWeaponInfo().getWeaponType() == WeaponType.Secondary)
        {
            secondaryWeapon = weapon;
            secondaryWeapon.transform.SetParent(secondaryHolder.transform);
            secondaryWeapon.transform.localPosition = Vector3.zero;
            secondaryWeapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        currentActiveWeapon = weapon;
    }
}
