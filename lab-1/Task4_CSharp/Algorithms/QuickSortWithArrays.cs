using BenchmarkDotNet.Attributes;

namespace Task4_CSharp.Algorithms;

public class QuickSortWithArrays
{
    [Benchmark]
    public void InvokeSort(int[] arr, int left, int right)
    {
        if (left >= right) return;
        var pivot = Partition(arr, left, right);

        if (pivot > 1)
            InvokeSort(arr, left, pivot - 1);

        if (pivot + 1 < right)
            InvokeSort(arr, pivot + 1, right);
    }

    [Benchmark]
    public int Partition(int[] arr, int left, int right)
    {
        var pivot = arr[left];
        while (true)
        {
            while (arr[left] < pivot)
                left++;

            while (arr[right] > pivot)
                right--;

            if (left < right)
            {
                if (arr[left] == arr[right]) return right;
                (arr[left], arr[right]) = (arr[right], arr[left]);
            }
            else
            {
                return right;
            }
        }
    }
}