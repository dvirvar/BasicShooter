using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class IncrementalDropdown : Dropdown
{
    public double minValue;
    public double maxValue;
    public double incremental;
    public bool reveresedOrder = false;
    
    protected override void OnEnable()
    {
        if (minValue > maxValue)
        {
            throw new ArithmeticException("Min value can't be greater than max value");
        } else if (incremental == 0)
        {
            throw new ArithmeticException("incremental can't be zero");
        } else
        {
            build();
        }
    }

    public void build()
    {
        options.Clear();
        for(double i = minValue; i <= maxValue; i += incremental)
        {
            options.Add(new OptionData(i.ToString()));
        }
        if (reveresedOrder)
        {
            options.Reverse();
        }
    }

}
