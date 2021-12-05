using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic TableView to add or change items inside a table view
/// </summary>
/// <typeparam name="INFO">The class that represent the info</typeparam>
/// <typeparam name="DISPLAY">The class that displaying the info></typeparam>
public class TableView<INFO, DISPLAY> : MonoBehaviour where DISPLAY : Display<INFO> where INFO : IEquatable<INFO>
{
    [SerializeField] protected Transform contentTransform;
    [SerializeField] private GameObject rowPrefab;
    
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

    public void addInfo(INFO info, int index)
    {
        DISPLAY display = createAndFillInfoDisplay(info);
        display.transform.SetSiblingIndex(index);
        infosDisplay.Insert(index, display);
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
        infosDisplay.RemoveAt(index);
        Destroy(contentTransform.GetChild(index).gameObject);
        return info;
    }

    public virtual void removeAll()
    {
        infosDisplay.ForEach(infoDisplay =>
        {
            Destroy(infoDisplay.gameObject);
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
        DISPLAY display = Instantiate(rowPrefab).GetComponent<DISPLAY>();
        display.transform.SetParent(contentTransform, false);
        display.info = info;
        return display;
    }
}