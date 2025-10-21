using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    // extending the List<T> functionality to cycle through and get next item
    public static T GetNextItem<T>(this List<T> list, T currentItem)
    {
        if (list != null || list.Any())
        {
            if (currentItem == null)
            {
                return list[0];
            }
            int currentIndex = list.IndexOf(currentItem);
            int maxIndex = list.Count - 1;
            if (currentIndex < maxIndex) // still left
            {
                return list[currentIndex + 1];
            }
            else // current was the last one
            {
                return list[0];
            }
        }
        return default(T);
    }
    
    // check if layer is in layermask
    public static bool CheckLayer(LayerMask layerMask, int layer)
    {
        if ((layerMask.value & (1 << layer)) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // add new item to simple array, if item alread in - skip, if array already full - skip
    public static bool TryAddNewItem<T>(this T[] array, T newItem)
    {
        if (array == null) return false;
        bool result = false;
        int length = array.Length;
        int index = 0;
        int emptyIndex = -1;
        T currentItem = default(T);
        while (index < length)
        {
            currentItem = array[index];
            if (currentItem == null)
            {
                emptyIndex = index;
            }
            // item already exists - break with false result
            else if (currentItem.Equals(newItem))
            {
                break;
            }
            index++;
        }
        if (emptyIndex > -1)
        {
            array[emptyIndex] = newItem;
            result = true;
        }
        return result;
    }


}
