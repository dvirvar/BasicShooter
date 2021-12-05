using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// A TableView that can be sorted by ascending/descending
/// </summary>
/// <typeparam name="I"></typeparam>
/// <typeparam name="D"></typeparam>
public abstract class SortedTableView<I,D> : TableView<I,D> where I: IEquatable<I> where D: Display<I>
{
    protected abstract Direction direction { get; }
    protected enum Direction
    {
        Ascending, Descending
    }

    public override void setInfos(List<I> infos)
    {
        infos.Sort(comparison);
        base.setInfos(infos);
    }

    public override void addInfo(I info)
    {
        if (direction == Direction.Ascending)
        {
            base.addInfo(info,0);
        } else
        {
            base.addInfo(info);
        }
    }

    protected void addSortPoints(string infoId, int points)
    {
        var listCount = infosDisplay.Count;
        for (int i = 0; i < listCount; i++)
        {
            var display = infosDisplay[i];
            if (isInfoEqual(infoId, display.info))
            {
                var currentPoints = addSortPoints(display.info, points);
                display.refreshDisplay();
                var positivePoints = points > 0;
                var index = i;
                while (true)
                {
                    nextIndex(ref index, positivePoints);
                    if (index >= infosDisplay.Count || index < 0)
                    {
                        return;
                    }
                    var otherPoints = sortedPointsOf(infosDisplay[index].info);
                    var needToChangePosition = positivePoints ? otherPoints >= currentPoints : otherPoints <= currentPoints;
                    if (needToChangePosition)
                    {
                        previousIndex(ref index, positivePoints);
                        infosDisplay.RemoveAt(i);
                        display.transform.SetSiblingIndex(index);
                        infosDisplay.Insert(index, display);
                        return;
                    }
                    else if (endOfArray(positivePoints, index))
                    {
                        infosDisplay.RemoveAt(i);
                        display.transform.SetSiblingIndex(index);
                        infosDisplay.Insert(index, display);
                        return;
                    }
                }
            }
        }
    }
    private bool endOfArray(bool isPositivePoints, int index)
    {
        if (direction == Direction.Ascending)
        {
            return (!isPositivePoints && index == 0) || (isPositivePoints && index == infosDisplay.Count - 1);
        } else
        {
            return (isPositivePoints && index == 0) || (!isPositivePoints && index == infosDisplay.Count - 1);
        }
    }
    
    private void previousIndex(ref int index, bool isPositivePoints)
    {
        nextIndex(ref index, !isPositivePoints);
    }
    private void nextIndex(ref int index, bool isPositivePoints)
    {
        if (direction == Direction.Ascending)
        {
            _ = isPositivePoints ? ++index : --index;
        } else
        {
            _ = isPositivePoints ? --index : ++index;
        }
    }
    /// <summary>
    /// Return if the infoId is equals to the other info
    /// </summary>
    /// <param name="infoId">The info Id</param>
    /// <param name="otherInfo">The other info</param>
    /// <returns>If info is equal to id</returns>
    protected abstract bool isInfoEqual(string infoId, I otherInfo);
    /// <summary>
    /// Add the points to the value that need to be sorted
    /// And return the value
    /// </summary>
    /// <param name="info">The info that needs to be updated</param>
    /// <param name="points">The points to add</param>
    /// <returns>The value that is sorted</returns>
    protected abstract int addSortPoints(I info , int points);
    /// <summary>
    /// Returns the sorted points of the desired info
    /// </summary>
    /// <param name="info">The info that holds the sorted points</param>
    /// <returns>The sorted points</returns>
    protected abstract int sortedPointsOf(I info);
    private int comparison(I info1, I info2)
    {
        return sortedPointsOf(info2).CompareTo(sortedPointsOf(info1));
    }
}
