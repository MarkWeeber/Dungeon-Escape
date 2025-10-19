using System.Collections.Generic;
using System.Linq;

public static class Utlis
{
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
}
