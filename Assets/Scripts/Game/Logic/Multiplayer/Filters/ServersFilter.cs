using System.Collections.Generic;
using UnityEngine;
using System;

public class ServersFilter : MonoBehaviour
{

    public event Action<List<ServerInfo>> onFilterChange = delegate { };

    [SerializeField]
    private GameModesFilter gameModesFilter;
    [SerializeField]
    private MapsFilter mapsFilter;
    [SerializeField]
    private FreeSlotsFilter freeSlotsFilter;
    [SerializeField]
    private GameSizeFilter gameSizeFilter;
    private FilterInfo filterInfo;

    public List<ServerInfo> serversInfo
    {
        get
        {
            return _serversInfo;
        }

        set
        {
            _serversInfo = value;
            filterServers();
        }
    }
    private List<ServerInfo> _serversInfo;

    private void Awake()
    {
        gameModesFilter.filterChanged += filterServers;
        mapsFilter.filterChanged += filterServers;
        freeSlotsFilter.filterChanged += filterServers;
        gameSizeFilter.filterChanged += filterServers;

        gameModesFilter.filterChanged += gameModesFilterChanged;
        mapsFilter.filterChanged += mapsFilterChanged;
        freeSlotsFilter.filterChanged += freeSlotsFilterChanged;
        gameSizeFilter.filterChanged += gameSizeFilterChanged;

        filterInfo = UserPrefs.getFilter();
    }

    private void Start() {
        if (filterInfo == null)
        {
            filterInfo = new FilterInfo();
            return;
        }
        gameModesFilter.updateFilters(filterInfo.getGameModeFilterTypes());
        mapsFilter.updateFilters(filterInfo.getMapFilterTypes());
        freeSlotsFilter.updateFilters(filterInfo.getFreeSlotsFilterTypes());
        gameSizeFilter.updateFilters(filterInfo.getGameSizeFilterTypes());
    }

    private void setCurrentFilterInfo(FilterInfo filterInfo) {
        UserPrefs.setFilter(filterInfo);
    }

    private void gameModesFilterChanged()
    {
        filterInfo.setGameModeFilterTypes(gameModesFilter.getSelectedTypes());
        setCurrentFilterInfo(filterInfo);
    }

    private void mapsFilterChanged()
    {
        filterInfo.setMapFilterTypes(mapsFilter.getSelectedTypes());
        setCurrentFilterInfo(filterInfo);
    }

    private void freeSlotsFilterChanged()
    {
        filterInfo.setFreeSlotsFilterTypes(freeSlotsFilter.getSelectedTypes());
        setCurrentFilterInfo(filterInfo);
    }

    private void gameSizeFilterChanged()
    {
        filterInfo.setGameSizeFilterTypes(gameSizeFilter.getSelectedTypes());
        setCurrentFilterInfo(filterInfo);
    }

    private void filterServers() {
        List<ServerInfo> filteredServersInfo = gameModesFilter.filter(_serversInfo);
        filteredServersInfo = mapsFilter.filter(filteredServersInfo);
        filteredServersInfo = freeSlotsFilter.filter(filteredServersInfo);
        filteredServersInfo = gameSizeFilter.filter(filteredServersInfo);
        onFilterChange(filteredServersInfo);
    }

    private void OnDestroy()
    {
        gameModesFilter.filterChanged -= filterServers;
        mapsFilter.filterChanged -= filterServers;
        freeSlotsFilter.filterChanged -= filterServers;
        gameSizeFilter.filterChanged -= filterServers;

        gameModesFilter.filterChanged -= gameModesFilterChanged;
        mapsFilter.filterChanged -= mapsFilterChanged;
        freeSlotsFilter.filterChanged -= freeSlotsFilterChanged;
        gameSizeFilter.filterChanged -= gameSizeFilterChanged;
    }
}
