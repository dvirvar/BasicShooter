using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPartsListView : MonoBehaviour
{
    [SerializeField] private InputField searchWeaponIF;
    [SerializeField] private WeaponPartsTableView weaponPartsTV;
    private List<WeaponPartItem> weaponPartItems;

    private void Awake()
    {
        weaponPartItems = new List<WeaponPartItem>();
        searchWeaponIF.onValueChanged.AddListener(delegate
        {
            if (searchWeaponIF.text == "")
            {
                showAll();
            } else
            {
                filterAll();
            }
        });
    }

    private void showAll()
    {
        weaponPartsTV.getDisplays().ForEach(wpd =>
        {
            wpd.gameObject.SetActive(true);
        });
    }

    private void filterAll()
    {
        showAll();
        weaponPartsTV.getDisplays().ForEach(wpd =>
        {
            if (!wpd.info.searchKeyWord.Contains(searchWeaponIF.text)) {
                wpd.gameObject.SetActive(false);
            }
        });
    }


    public void setWeaponPartItems(List<WeaponPartItem> weaponPartItems)
    {
        this.weaponPartItems = weaponPartItems;
        weaponPartsTV.setInfos(weaponPartItems);
    }

    public List<WeaponPartDisplay> getDisplays()
    {
        return weaponPartsTV.getDisplays();
    }

    public void clear()
    {
        searchWeaponIF.text = "";
        weaponPartsTV.removeAll();
        weaponPartItems.Clear();
    }

    private void OnDestroy()
    {
        searchWeaponIF.onValueChanged.RemoveAllListeners();
    }
}
