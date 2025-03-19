using System;
using System.Linq;

public class ArrayUtils
{
    public static int SumArray(int[] arr)
    {
        if (arr == null)
        {
            throw new ArgumentException("Mảng không được null");
        }
        return arr.Sum();
    }
}
