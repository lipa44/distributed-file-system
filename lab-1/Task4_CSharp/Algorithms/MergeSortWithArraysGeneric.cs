using BenchmarkDotNet.Attributes;

namespace Task4_CSharp.Algorithms;

public class MergeGenericSortWithArraysGeneric : IGenericSort<int[]>
{
    [Benchmark]
    public int[] InvokeSort(int[] array)
    {
        int[] left, right, result = new int[array.Length];

        if (array.Length <= 1)
            return array;

        var midPoint = array.Length / 2;
        left = new int[midPoint];

        if (array.Length % 2 == 0)
            right = new int[midPoint];
        else
            right = new int[midPoint + 1];

        for (var i = 0; i < midPoint; i++)
            left[i] = array[i];

        int x = 0;

        for (var i = midPoint; i < array.Length; i++)
        {
            right[x] = array[i];
            x++;
        }


        left = InvokeSort(left);
        right = InvokeSort(right);
        result = Merge(left, right);
        return result;
    }

    [Benchmark]
    public int[] Merge(int[] left, int[] right)
    {
        var resultLength = right.Length + left.Length;
        var result = new int[resultLength];
        int indexLeft = 0, indexRight = 0, indexResult = 0;

        while (indexLeft < left.Length || indexRight < right.Length)
        {
            if (indexLeft < left.Length && indexRight < right.Length)
            {
                if (left[indexLeft] <= right[indexRight])
                {
                    result[indexResult] = left[indexLeft];
                    indexLeft++;
                    indexResult++;
                }
                else
                {
                    result[indexResult] = right[indexRight];
                    indexRight++;
                    indexResult++;
                }
            }
            else if (indexLeft < left.Length)
            {
                result[indexResult] = left[indexLeft];
                indexLeft++;
                indexResult++;
            }
            else if (indexRight < right.Length)
            {
                result[indexResult] = right[indexRight];
                indexRight++;
                indexResult++;
            }
        }

        return result;
    }
}