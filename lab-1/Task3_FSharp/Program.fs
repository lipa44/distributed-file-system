open Task3_CSharp

let vertices = System.Collections.Generic.List<int>()
vertices.Add(1);
vertices.Add(2);
vertices.Add(3);
vertices.Add(4);
vertices.Add(5);
vertices.Add(6);
vertices.Add(7);
vertices.Add(8);
vertices.Add(9);
vertices.Add(10)

let edges = System.Collections.Generic.List<Edge<int>>()
edges.Add(Edge<int>(1, 2))
edges.Add(Edge<int>(1, 3))
edges.Add(Edge<int>(2, 3))
edges.Add(Edge<int>(3, 5))
edges.Add(Edge<int>(3, 6))
edges.Add(Edge<int>(4, 7))
edges.Add(Edge<int>(5, 7))
edges.Add(Edge<int>(5, 8))
edges.Add(Edge<int>(5, 6))
edges.Add(Edge<int>(8, 9))
edges.Add(Edge<int>(9, 10))

let graph = new Graph<int>(vertices, edges);
let algorithm = new Algorithms();
            
let hashSet = algorithm.Bfs(graph, 0)

printf 