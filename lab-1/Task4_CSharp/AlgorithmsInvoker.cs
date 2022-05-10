using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Order;
using Task4_CSharp.Algorithms;

namespace Task4_CSharp;

[RPlotExporter]
[MemoryDiagnoser]
[EventPipeProfiler(EventPipeProfile.GcCollect)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class AlgorithmsInvoker
{
    private static readonly IEnumerable<int> Data = Enumerable
        .Repeat(0, 10)
        .Select(i => new Random().Next(0, 10000));

    [Benchmark]
    public void InvokeMergeGenericSortWithArraysGeneric()
    {
        new MergeGenericSortWithArraysGeneric().InvokeSort(Data.ToArray());
    }

    [Benchmark]
    public void InvokeMergeGenericSortWithListsGeneric()
    {
        new MergeGenericSortWithListsGeneric().InvokeSort(Data.ToList());
    }

    [Benchmark]
    public void InvokeMergeSortWithArraysNonGeneric()
    {
        new MergeSortWithArraysNonGeneric().InvokeSort(Data.ToArray());
    }

    [Benchmark]
    public void InvokeMergeSortWithListsNonGeneric()
    {
        new MergeSortWithListsNonGeneric().InvokeSort(Data.ToList());
    }

    [Benchmark]
    public void InvokeBuiltInQuickSort()
    {
        Data.ToList().Sort();
    }
}