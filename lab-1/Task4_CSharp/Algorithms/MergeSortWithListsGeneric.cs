using BenchmarkDotNet.Attributes;

namespace Task4_CSharp.Algorithms;

public class MergeGenericSortWithListsGeneric : IGenericSort<List<int>>
{
    [Benchmark]
    public List<int> InvokeSort(List<int> unsorted)
    {
        if (unsorted.Count <= 1)
            return unsorted;

        List<int> left = new ();
        List<int> right = new ();

        var middle = unsorted.Count / 2;
        for (var i = 0; i < middle; i++)
            left.Add(unsorted[i]);

        for (var i = middle; i < unsorted.Count; i++)
            right.Add(unsorted[i]);

        left = InvokeSort(left);
        right = InvokeSort(right);
        return Merge(left, right);
    }

    [Benchmark]
    private List<int> Merge(List<int> left, List<int> right)
    {
        List<int> result = new ();

        while (left.Count > 0 || right.Count > 0)
        {
            switch (left.Count)
            {
                case > 0 when right.Count > 0:
                {
                    if (left.First() <= right.First())
                    {
                        result.Add(left.First());
                        left.Remove(left.First());
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }

                    break;
                }
                case > 0:
                    result.Add(left.First());
                    left.Remove(left.First());
                    break;
                default:
                {
                    if (right.Count > 0)
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }

                    break;
                }
            }
        }

        return result;
    }
}