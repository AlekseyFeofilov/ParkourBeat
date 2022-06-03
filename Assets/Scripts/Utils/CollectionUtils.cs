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
    }
}