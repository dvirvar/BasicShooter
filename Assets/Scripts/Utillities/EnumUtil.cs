using ExtensionMethods;
using System;
using System.Collections.Generic;


public class EnumUtil
{
    public static T[] getListOf<T>() where T: Enum
    {
        return (T[])Enum.GetValues(typeof(T));
    }
    public static List<string> getListOfDescriptions<T>() where T : Enum
    {
        var list = new List<string>();
        foreach (T t in getListOf<T>())
        {
            list.Add(t.GetDescription());
        }
        return list;
    }
}
