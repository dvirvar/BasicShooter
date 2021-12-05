using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField] private Text weaponNameText, magazineText, bulletsLeftText, weaponModeText, pingText;
    [SerializeField] private LoadoutHolder loadoutHolder;

    private Weapon currentWeapon;

    private void Start()
    {
        loadoutHolder.onWeaponChanged += WeaponHolder_onWeaponChanged;
        WeaponHolder_onWeaponChanged(loadoutHolder.currentActiveWeapon);
    }

    private void WeaponHolder_onWeaponChanged(Weapon obj)
    {
        if (currentWeapon != null)
        {
            currentWeapon.bulletsInMagazineChanged -= bulletsInMagazineChanged;
            currentWeapon.bulletsChanged -= bulletsChanged;
            currentWeapon.weaponModeChanged -= weaponModeChanged;
        }
        currentWeapon = obj;
        if (currentWeapon == null)
        {
            bulletsChanged(0);
            bulletsInMagazineChanged(0);
            weaponNameText.text = "None";
            return;
        }
        bulletsChanged(currentWeapon.bullets);
        bulletsInMagazineChanged(currentWeapon.bulletsInMagazine);
        weaponModeChanged(currentWeapon.getCurrentWeaponMode());
        weaponNameText.text = currentWeapon.getWeaponInfo().getID().GetDescription();
        currentWeapon.bulletsInMagazineChanged += bulletsInMagazineChanged;
        currentWeapon.bulletsChanged += bulletsChanged;
        currentWeapon.weaponModeChanged += weaponModeChanged;
    }
    
    private void bulletsChanged(int bullets)
    {
        bulletsLeftText.text = bullets.ToString();
    }

    private void bulletsInMagazineChanged(int bullets)
    {
        magazineText.text = bullets.ToString();
    }

    private void weaponModeChanged(WeaponModes weaponModes)
    {
        weaponModeText.text = weaponModes.getString();
    }

    public void setPing(int ping) {
        pingText.text = ping.ToString();
    }

    private void OnDestroy()
    {
        loadoutHolder.onWeaponChanged -= WeaponHolder_onWeaponChanged;
        if (currentWeapon != null)
        {
            currentWeapon.bulletsInMagazineChanged -= bulletsInMagazineChanged;
            currentWeapon.bulletsChanged -= bulletsChanged;
            currentWeapon.weaponModeChanged -= weaponModeChanged;
        }
    }
}
