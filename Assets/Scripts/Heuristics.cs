using UnityEngine;

public class Heuristics : MonoBehaviour {
    public bool manhattanHeuristics;
    public bool euclideanHeuristics;

    public void init(Graph graph, Node goalNode) {
        StaticHeuristics.manhattanStaticBool = manhattanHeuristics;
        StaticHeuristics.euclideanStaticBool = euclideanHeuristics;

        if (graph != null && goalNode != null) {
            if (manhattanHeuristics) {
                assignManhattanDistance(graph, goalNode);
            } else if (euclideanHeuristics) {
                assignEuclideanDistance(graph, goalNode);
            }
        }
    }

    // calculate and assign the distances
    private void assignManhattanDistance(Graph graph, Node goalNode) {
        foreach (Node node in graph.nodes) {
            node.distance = calculateManhattanDistance(node, goalNode);
        }
    }

    private void assignEuclideanDistance(Graph graph, Node goalNode) {
        foreach (Node node in graph.nodes) {
            node.distance = calculateEuclideanDistance(node, goalNode);
        }
    }

    private int calculateManhattanDistance(Node someNode, Node goalNode) {
        return Mathf.Abs(someNode.xIndex - goalNode.xIndex) + Mathf.Abs(someNode.yIndex - goalNode.yIndex);
    }

    public int calculateEuclideanDistance(Node someNode, Node goalNode) {
        return (int)Mathf.Sqrt(Mathf.Pow(someNode.xIndex - goalNode.xIndex, 2) + Mathf.Pow(someNode.yIndex - goalNode.yIndex, 2));
    }

    public static class StaticHeuristics {
        public static bool manhattanStaticBool;
        public static bool euclideanStaticBool;
    }
}
