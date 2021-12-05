using System.Collections.Generic;

public class FilterInfo
{
    public GameModeType[] gameModeTypes;
    public MapType[] mapTypes;
    public FreeSlotsFilterType[] freeSlotsFilterTypes;
    public GameSizeFilterType[] gameSizeFilterTypes;

    public FilterInfo()
    {
        gameModeTypes = new GameModeType[0];
        mapTypes = new MapType[0];
        freeSlotsFilterTypes = new FreeSlotsFilterType[0];
        gameSizeFilterTypes = new GameSizeFilterType[0];
    }

    public void setGameModeFilterTypes(HashSet<GameModeType> filterTypes)
    {
        gameModeTypes = new GameModeType[filterTypes.Count];
        filterTypes.CopyTo(gameModeTypes);
    }

    public void setMapFilterTypes(HashSet<MapType> filterTypes)
    {
        mapTypes = new MapType[filterTypes.Count];
        filterTypes.CopyTo(mapTypes);
    }

    public void setFreeSlotsFilterTypes(HashSet<FreeSlotsFilterType> filterTypes)
    {
        freeSlotsFilterTypes = new FreeSlotsFilterType[filterTypes.Count];
        filterTypes.CopyTo(freeSlotsFilterTypes);
    }

    public void setGameSizeFilterTypes(HashSet<GameSizeFilterType> filterTypes)
    {
        gameSizeFilterTypes = new GameSizeFilterType[filterTypes.Count];
        filterTypes.CopyTo(gameSizeFilterTypes);
    }

    public HashSet<GameModeType> getGameModeFilterTypes()
    {
        return new HashSet<GameModeType>(gameModeTypes);
    }

    public HashSet<MapType> getMapFilterTypes()
    {
        return new HashSet<MapType>(mapTypes);
    }

    public HashSet<FreeSlotsFilterType> getFreeSlotsFilterTypes()
    {
        return new HashSet<FreeSlotsFilterType>(freeSlotsFilterTypes);
    }

    public HashSet<GameSizeFilterType> getGameSizeFilterTypes()
    {
        return new HashSet<GameSizeFilterType>(gameSizeFilterTypes);
    }
}
