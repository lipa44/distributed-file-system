using System.Collections.Generic;

namespace Task3_CSharp;

public class Graph<T>
{
    public Graph(List<T> vertices, List<Edge<T>> edges)
    {
        foreach (var vertex in vertices)
            AddVertex(vertex);

        foreach (var edge in edges)
            AddEdge(edge);
    }

    public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new();

    public void AddVertex(T vertex)
    {
        AdjacencyList[vertex] = new HashSet<T>();
    }

    public void AddEdge(Edge<T> edge)
    {
        if (AdjacencyList.ContainsKey(edge.First) && AdjacencyList.ContainsKey(edge.Second))
        {
            AdjacencyList[edge.First].Add(edge.Second);
            AdjacencyList[edge.Second].Add(edge.First);
        }
    }
}