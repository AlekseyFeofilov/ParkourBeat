using System;
using System.Collections.Generic;

namespace Utils
{
    public static class CollectionUtils
    {
        public static int SearchBinaryInRangeList<T>(IList<T> list, Func<T, double> func, double element)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right) {
                int mid = (left + right) / 2;
                double midElement = func.Invoke(list[mid]);
                
                if (element >= midElement && 
                    (mid + 1 >= list.Count || element < func.Invoke(list[mid + 1])))
                {
                    return mid;
                }
                
                if (element < midElement) 
                {
                    right = mid - 1;
                }
                else 
                {
                    left = mid + 1;
                }
            }
            return -1;
        }

        public static int FindPrevoius<T>(IList<T> list, int index, Func<T, bool> condition)
        {
            for (int i = Math.Min(index - 1, list.Count - 1); i >= 0; i--)
            {
                if (condition.Invoke(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindNext<T>(IList<T> list, int index, Func<T, bool> condition)
        {
            for (int i = Math.Max(index + 1, 0); i < list.Count; i++)
            {
                if (condition.Invoke(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static Dictionary<int, T> AssociateByIndex<T>(IList<T> list)
        {
            Dictionary<int, T> dictionary = new();
            for (int i = 0; i < list.Count; i++)
            {
                dictionary[i] = list[i];
            }

            return dictionary;
        }
    }
}