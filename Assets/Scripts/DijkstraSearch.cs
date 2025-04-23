using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DijkstraSearch : MonoBehaviour {
    Node startNode;
    Node goalNode;
    Graph graph;
    GraphView graphView;
    List<Node> pathNodes;
    List<Node> frontierNodes;
    List<Node> exploredNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.grey;
    public Color pathColor = Color.cyan;
    public bool isComplete;
    public int iterations;

    public void init(Graph graph, GraphView graphView, Node start, Node goal) {
        if (start == null || goal == null || graph == null || graphView == null) {
            Debug.LogWarning("DFS init error: Missing Component");
            return;
        } else if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked) {
            Debug.LogWarning("Start node or goal node cannot be blocked!");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        startNode = start;
        goalNode = goal;

        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();
        frontierNodes = new List<Node>();
        
        for (int y = 0; y < graph.mapHeight; y++) {
            for (int x = 0; x < graph.mapWidth; x++) {
                graph.nodes[x, y].reset();
                graph.nodes[x, y].dijkstraDistance = int.MaxValue; // Initialize to infinity
            }
        }

        startNode.dijkstraDistance = 0; // distance from startNode to startNode
        frontierNodes.Add(startNode);

        showColors();

        isComplete = false;
        iterations = 0;
    }

    public IEnumerator searchRoutine(float timeStep = 0.1f) {
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                iterations++;

                // sorting to mimic a min heap
                quickSort(frontierNodes, 0, frontierNodes.Count - 1);

                Node currentNode = frontierNodes[0];
                frontierNodes.Remove(currentNode);
                exploredNodes.Add(currentNode);

                expandFrontier(currentNode); 

                if (frontierNodes.Contains(goalNode)) {
                    pathNodes = getPathNodes(goalNode);
                    isComplete = true;
                }

                yield return new WaitForSeconds(timeStep);
            } else {
                isComplete = true;
            }
            showColors();
        }
    }

    List<Node> getPathNodes(Node goalNode) {
        List<Node> path = new List<Node>();

        if (goalNode == null) {
            return path;
        }

        path.Add(goalNode);
        Node currentNode = goalNode.previous;
        while (currentNode != null) {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }

    private void showColors(GraphView graphView, Node start, Node goal) {
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];


        if (frontierNodes != null) {
            graphView.colorNodes(frontierNodes, frontierColor);
        }

        if (exploredNodes != null) {
            graphView.colorNodes(exploredNodes, exploredColor);
        }

        if (pathNodes != null) {
            graphView.colorNodes(pathNodes, pathColor);
        }

        if (startNodeView != null) {
            startNodeView.colorNode(startColor);
        }

        if (goalNodeView != null) {
            goalNodeView.colorNode(goalColor);
        }
    }

    public void showColors() {
        showColors(graphView, startNode, goalNode);
    }

    private void expandFrontier(Node node) {
        foreach(Node neighbor in node.neighbors) {
            if(node.dijkstraDistance + 1 < neighbor.dijkstraDistance) {
                neighbor.dijkstraDistance = node.dijkstraDistance + 1;

                if (!exploredNodes.Contains(neighbor) &&
                    !frontierNodes.Contains(neighbor)) {
                    neighbor.previous = node;
                    frontierNodes.Add(neighbor);
                }
            }
        }
    }

    private void quickSort(List<Node> frontierNodes, int low, int high) {
        if(low < high) {
            int pivotIndex = partition(frontierNodes, low, high);

            quickSort(frontierNodes, low, pivotIndex - 1);
            quickSort(frontierNodes, pivotIndex + 1, high);
        }
    }

    private int partition(List<Node> array, int low, int high) {
        Node pivot = array[high];
        int index = low - 1;

        for (int i = low; i <= high - 1; i++) {
            if (array[i].dijkstraDistance < pivot.dijkstraDistance) {
                index++;
                swap(array, index, i);
            }
        }

        // move pivot to end of sorted part
        swap(array, index + 1, high);

        return index + 1;
    }

    private void swap(List<Node> array, int firstIndex, int secondIndex) {
        Node temp = array[firstIndex];
        array[firstIndex] = array[secondIndex];
        array[secondIndex] = temp;
    }
}
