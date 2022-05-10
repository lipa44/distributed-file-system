using System;

namespace Task3_CSharp;

public class Edge<T>
{
    public Edge(T first, T second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        First = first;
        Second = second;
    }

    public T First { get; }

    public T Second { get; }
}