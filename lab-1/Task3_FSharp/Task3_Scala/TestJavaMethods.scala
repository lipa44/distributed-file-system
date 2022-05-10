class TestJavaMethods {
  def main(args: Array[String]): Unit = {
  }

  def testDfs(): Unit = {
    val graph = new SuperSmartGraphAutoDFS3000UltraMegaSuperGood
    graph.addVertex(0)
    graph.addVertex(1)
    graph.addVertex(2)
    graph.addVertex(3)
    graph.addVertex(4)
    graph.addVertex(5)
    graph.addEdge(0, 1)
    graph.addEdge(0, 2)
    graph.addEdge(1, 3)
    graph.addEdge(2, 3)
    graph.addEdge(3, 4)
    graph.addEdge(4, 5)
    graph.dfs(0);
  }
}

