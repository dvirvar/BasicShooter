using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Generic class that represent a multi selected filter
/// </summary>
/// <typeparam name="T">Filter type</typeparam>
/// <typeparam name="R">Filter Row</typeparam>
public abstract class MultiSelectedFilter<T,R> : MonoBehaviour where T: Enum where R: FilterRow<T>
{
    public event Action filterChanged = delegate { };

    [SerializeField]
    private GameObject filterPrefab;
    [SerializeField]
    private GameObject contentObj;//The content of filter rows also the toggle group

    protected HashSet<T> selectedTypes;
    protected List<R> filterRows;

    public bool multiSelected;

    protected virtual void Awake()
    {
        ToggleGroup toggleGroup = contentObj.GetComponent<ToggleGroup>();
        filterRows = new List<R>();
        selectedTypes = new HashSet<T>();
        foreach (T filterType in (T[])Enum.GetValues(typeof(T)))
        {
            R filterRow = Instantiate(filterPrefab, contentObj.transform).GetComponent<R>();
            filterRows.Add(filterRow);
            if (multiSelected)
            {
                filterRow.BuildRow(filterType, false);
            } else
            {
                filterRow.BuildRow(filterType, false, toggleGroup);
            }
            filterRow.toggleChanged += FilterRow_toggleChanged;
        }
    }

    private void FilterRow_toggleChanged(T filterType, bool toggled)
    {
        if (toggled)
        {
            selectedTypes.Add(filterType);
        }
        else
        {
            selectedTypes.Remove(filterType);
        }
        filterChanged();
    }

    public abstract List<ServerInfo> filter(List<ServerInfo> serversInfo);

    protected virtual void OnDestroy()
    {
        filterRows.ForEach(filterRow =>
        {
            filterRow.toggleChanged -= FilterRow_toggleChanged;
        });
    }
}
