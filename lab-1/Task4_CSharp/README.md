# Бенчмаркинг

## Задание

> Изучить инструменты для оценки производительности в C# и Java.
> Написать несколько алгоритмов сортировок (и взять стандартную) и запустить бенчмарки (в бенчмарках помимо времени выполнения проверить аллокации памяти).
> В отчёт написать про инструменты для бенчмаркинга, их особености, анализ результатов проверок.

## Теормин

**Бенчмаркинг** - это процесс измерения различных метрик работы методов и анализ их результатов с целью найти наиболее эффективное решенние.

## Решение

## Бенчмаркинг в C#

Бенчмаркинг в `C#` реализован с помощью библиотеки `BenchmarkDotNet`, в которую встроен весь функционал, нужный для оценки практически всевозможных метрик, **в том числе аллокация памяти**.

Для реализации бенчмаркинга нужно навесить на методы, метрики которых мы хотим вычислить, специальный аттрибут `[Benchmark]` - это будет сигналом о том, что метрики данного метода должны быть посчитаны.

Все методы, метрики которых мы хотим посчитать, должны находиться в классе, на который также можно навешивать различные аттрибуты для уточнения или расширения процесса бенчмаркинга.

### Аттрибуты и возможности

1. `[RPlotExporter]` - построение графика по результатом оценки производительности
2. `[MemoryDiagnoser]` - включение графы "аллокация памяти" в вычисляемые метрики
3. `[Orderer(SummaryOrderPolicy.FastestToSlowest)]` - сортировка результатов по наилучшему времени исполнения
4. `[EventPipeProfiler(EventPipeProfile.GcCollect)]` - включение метрики о собранном мусоре
5. `[RankColumn]` - присвоение методам порядкового номера по результатам бенчмаркинга в порядке ухудшения результатов

> Продолжать можно долго, мне лень

### Реализация

Я написал несколько алгоритмов сортировки, ключевыми отличиями были:

1. Наследование от `generic` интерфейса
2. Работа с `array`/`list`

```csharp
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
```

 ### Результаты бенчмарка C#

Приведённая таблица является результатом выполнения бенчмаркинга в режиме `VeryLongTerm` _(2,5 часа)_ на массивах, состоящих из 10 _(сори)_ рандомных значений типа `int`

```markdown
|                                  Method |         Job |  Runtime | IterationCount | LaunchCount | WarmupCount |     Mean |     Error |    StdDev |   Median | Rank |  Gen 0 | Allocated |
|---------------------------------------- |------------ |--------- |--------------- |------------ |------------ |---------:|----------:|----------:|---------:|-----:|-------:|----------:|
|                  InvokeBuiltInQuickSort | VeryLongRun | .NET 6.0 |            500 |           4 |          30 | 1.299 μs | 0.0135 μs | 0.1789 μs | 1.287 μs |    1 | 0.4044 |     848 B |
| InvokeMergeGenericSortWithArraysGeneric | VeryLongRun | .NET 6.0 |            500 |           4 |          30 | 1.387 μs | 0.0008 μs | 0.0097 μs | 1.385 μs |    1 | 1.2035 |   2,520 B |
|     InvokeMergeSortWithArraysNonGeneric | VeryLongRun | .NET 6.0 |            500 |           4 |          30 | 1.398 μs | 0.0013 μs | 0.0162 μs | 1.394 μs |    1 | 1.2035 |   2,520 B |
|  InvokeMergeGenericSortWithListsGeneric | VeryLongRun | .NET 6.0 |            500 |           4 |          30 | 3.340 μs | 0.0028 μs | 0.0366 μs | 3.328 μs |    1 | 1.5221 |   3,184 B |
|      InvokeMergeSortWithListsNonGeneric | VeryLongRun | .NET 6.0 |            500 |           4 |          30 | 3.544 μs | 0.0512 μs | 0.6672 μs | 3.329 μs |    1 | 1.5221 |   3,184 B |
```



