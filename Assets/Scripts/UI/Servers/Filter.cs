using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Generic class that represent a multi/single selected filter
/// </summary>
/// <typeparam name="T">Filter type</typeparam>
/// <typeparam name="R">Filter Row</typeparam>
public abstract class Filter<T,R> : MonoBehaviour where T: Enum where R: FilterRow<T>
{
    public event Action filterChanged = delegate { };

    [SerializeField]
    private GameObject filterPrefab;
    [SerializeField]
    private GameObject contentObj;//The content of the filter rows and also the toggle group
    [SerializeField]
    private bool isMultiSelected;

    protected HashSet<T> selectedTypes;
    protected List<R> filterRows;
    
    protected virtual void Awake()
    {
        ToggleGroup toggleGroup = contentObj.GetComponent<ToggleGroup>();
        filterRows = new List<R>();
        selectedTypes = new HashSet<T>();
        foreach (T filterType in (T[])Enum.GetValues(typeof(T)))
        {
            R filterRow = Instantiate(filterPrefab, contentObj.transform).GetComponent<R>();
            filterRows.Add(filterRow);
            if (isMultiSelected)
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

    public void updateFilters(HashSet<T> filterTypes)
    {
        foreach (T filterType in filterTypes) {
            foreach (R filterRow in filterRows) {
                if (filterRow.filterType.Equals(filterType)) {
                    filterRow.ChangeToggle(true);
                    break;
                }
            }
        }
    }

    public HashSet<T> getSelectedTypes() {
        return selectedTypes;
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
