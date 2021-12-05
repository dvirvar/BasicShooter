using System.Collections.Generic;
using System;

public class GunMasterTableView : TableView<GunMasterRowInfo, GunMasterDisplay>
{
    public event Action<string> OnItemHover = delegate { };
    public event Action OnItemFinishHover = delegate { };
    public override void setInfos(List<GunMasterRowInfo> infos)
    {
        base.setInfos(infos);
        foreach (var item in getDisplays())
        {
            item.OnBeganHovering += Item_OnBeganHovering;
            item.OnStoppedHovering += Item_OnStoppedHovering;
        }
    }

    private void Item_OnBeganHovering(string obj)
    {
        OnItemHover(obj);
    }

    private void Item_OnStoppedHovering()
    {
        OnItemFinishHover();
    }

    public override void removeAll()
    {
        foreach (var item in getDisplays())
        {
            item.OnBeganHovering -= Item_OnBeganHovering;
            item.OnStoppedHovering -= Item_OnStoppedHovering;
        }
        base.removeAll();
    }
}
