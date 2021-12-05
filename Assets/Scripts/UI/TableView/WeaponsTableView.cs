using System.Collections.Generic;
using System;

public class WeaponsTableView : TableView<WeaponListItem,CustomizationWeaponDisplay>
{
    public event Action<CustomizableWeapon> OnItemClicked = delegate { };
    public event Action<bool, CustomizableWeapon> OnItemHover = delegate { };
    private CustomizationWeaponDisplay chosenWeapondisplay;

    public void setChosenWeapon(WeaponInfo weapon)
    {
        chosenWeapondisplay?.setChosen(false);
        foreach (var cwd in getDisplays())
        {
            if (cwd.info.weaponInfo.getID() == weapon.getID())
            {
                cwd.setChosen(true);
                chosenWeapondisplay = cwd;
                break;
            }
        }
    }

    public override void removeAll()
    {
        chosenWeapondisplay = null;
        getDisplays().ForEach(cwd => {
            cwd.OnClick -= Cwd_OnClick;
            cwd.OnHover -= Cwd_OnHover;
        });
        base.removeAll();
    }

    public override void setInfos(List<WeaponListItem> infos)
    {
        base.setInfos(infos);
        getDisplays().ForEach(cwd =>
        {
            cwd.OnClick += Cwd_OnClick;
            cwd.OnHover += Cwd_OnHover;
        });
    }

    public void setInfos(List<WeaponListItem> infos, WeaponInfo chosenWeapon)
    {
        base.setInfos(infos);
        getDisplays().ForEach(cwd =>
        {
            if (cwd.info.weaponInfo.getID() == chosenWeapon.getID())
            {
                cwd.setChosen(true);
                chosenWeapondisplay = cwd;
            }
            cwd.OnClick += Cwd_OnClick;
            cwd.OnHover += Cwd_OnHover;
        });
    }

    private void Cwd_OnClick(CustomizableWeapon obj)
    {
        OnItemClicked(obj);
    }

    private void Cwd_OnHover(bool isHover, CustomizableWeapon obj)
    {
        OnItemHover(isHover, obj);
    }
}
