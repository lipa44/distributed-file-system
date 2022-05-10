import java.util.*;

public class SuperSmartGraphAutoDFS3000UltraMegaSuperGood {

    private final Map<Integer, List<Integer>> adjVertices;

    public SuperSmartGraphAutoDFS3000UltraMegaSuperGood() {
        adjVertices = new HashMap<>();
    }

    public void addVertex(int vertex) {
        adjVertices.putIfAbsent(vertex, new ArrayList<>());
    }

    public void addEdge(int src, int dest) {
        adjVertices.get(src).add(dest);
    }

    public void dfs(int start) {
        boolean[] isVisited = new boolean[adjVertices.size()];
        dfsRecursive(start, isVisited);
    }

    private void dfsRecursive(int current, boolean[] isVisited) {
        isVisited[current] = true;
        visit(current);
        for (int dest : adjVertices.get(current)) {
            if (!isVisited[dest])
                dfsRecursive(dest, isVisited);
        }
    }

    private void visit(int value) {
        System.out.print(" " + value);
    }
}