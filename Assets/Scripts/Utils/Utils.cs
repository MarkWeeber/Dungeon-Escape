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
}
