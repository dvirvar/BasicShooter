using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic PooledTableView to add or change items inside a table view but through pooling instead of allocation
/// </summary>
/// <typeparam name="INFO">The class that represent the info</typeparam>
/// <typeparam name="DISPLAY">The class that displaying the info></typeparam>
public class PooledTableView<INFO, DISPLAY> : MonoBehaviour where DISPLAY : PooledDisplay<INFO> where INFO : IEquatable<INFO>
{
    [SerializeField] protected Transform contentTransform;
    protected virtual PoolObjectID poolObjectID { get;}

    protected List<DISPLAY> infosDisplay;

    protected virtual void Awake()
    {
        infosDisplay = new List<DISPLAY>();
    }

    public List<DISPLAY> getDisplays()
    {
        return infosDisplay;
    }

    public DISPLAY getDisplay(int index)
    {
        return infosDisplay[index];
    }

    public virtual void setInfos(List<INFO> infos)
    {
        removeAll();
        infos.ForEach(info =>
        {
            addInfo(info);
        });
    }

    public virtual void addInfo(INFO info)
    {
        infosDisplay.Add(createAndFillInfoDisplay(info));
    }

    public DISPLAY addInfo(INFO info, int index)
    {
        DISPLAY display = createAndFillInfoDisplay(info);
        display.transform.SetSiblingIndex(index);
        infosDisplay.Insert(index, display);
        return display;
    }

    public void changeInfo(INFO info, int index)
    {
        infosDisplay[index].info = info;
    }

    public void changeIndex(int from, int to)
    {
        if (from == to)
        {
            return;
        }
        var fromDis = infosDisplay[from];
        fromDis.transform.SetSiblingIndex(to);
        infosDisplay.RemoveAt(from);
        infosDisplay.Insert(to, fromDis);
    }

    public void removeInfo(INFO info)
    {
        for (int i = 0; i < infosDisplay.Count; i++)
        {
            if (infosDisplay[i].info.Equals(info))
            {
                removeInfoAt(i);
                return;
            }
        }
    }

    public INFO removeInfoAt(int index)
    {
        var info = getInfoAt(index);
        GameObjectsPool.instance.moveToPool(infosDisplay[index]);
        infosDisplay.RemoveAt(index);
        return info;
    }

    public virtual void removeAll()
    {
        infosDisplay.ForEach(infoDisplay =>
        {
            GameObjectsPool.instance.moveToPool(infoDisplay);
        });
        infosDisplay.Clear();
    }

    public int getIndexOfInfo(INFO info)
    {
        for (int i = 0; i < infosDisplay.Count; i++)
        {
            if (infosDisplay[i].info.Equals(info))
            {
                return i;
            }
        }
        return -1;
    }

    public INFO getInfoAt(int index)
    {
        return infosDisplay[index].info;
    }

    protected virtual DISPLAY createAndFillInfoDisplay(INFO info)
    {
        DISPLAY display = GameObjectsPool.instance.get(poolObjectID).GetComponent<DISPLAY>();
        display.transform.SetParent(contentTransform, false);
        display.info = info;
        return display;
    }
}
