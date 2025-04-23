using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PathFinder : MonoBehaviour {
    Node startNode;
    Node goalNode;
    Graph graph;
    GraphView graphView;
    Queue<Node> frontierNodes; // list of nodes that have to be explored
    List<Node> exploredNodes; // list of nodes that have been explored
    List<Node> pathNodes;
    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.grey;
    public Color pathColor = Color.cyan;
    public bool isComplete;
    public int iterations;

    public void init(Graph graph, GraphView graphView, Node start, Node goal) {
        if (start == null || goal == null || graph == null || graphView == null) {
            Debug.LogWarning("Path Finder init error: Missing Component");
            return;
        } else if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked) {
            Debug.LogWarning("Start node or goal node cannot be blocked!");
            return;
        }

        this.graph = graph;
        this.graphView = graphView;
        startNode = start;
        goalNode = goal;

        frontierNodes = new Queue<Node>();
        frontierNodes.Enqueue(startNode);
        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();
        for (int y = 0; y < graph.mapHeight; y++) {
            for (int x = 0; x < graph.mapWidth; x++) {
                graph.nodes[x, y].reset();
            }
        }

        showColors();

        isComplete = false;
        iterations = 0;
    }

    

    private void showColors(GraphView graphView, Node start, Node goal) { 
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];


        if (frontierNodes != null) {
            graphView.colorNodes(frontierNodes.ToList(), frontierColor);
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

    public IEnumerator searchRoutine(float timeStep = 0.1f) {
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                Node currentNode = frontierNodes.Dequeue();
                iterations++;

                if (!exploredNodes.Contains(currentNode)) {
                    exploredNodes.Add(currentNode);
                }
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

        if(goalNode == null) {
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

    private void expandFrontier(Node node) {
        for (int i = 0; i < node.neighbors.Count; i++) {
            if (!exploredNodes.Contains(node.neighbors[i]) &&
                !frontierNodes.Contains(node.neighbors[i])) {
                node.neighbors[i].previous = node;
                frontierNodes.Enqueue(node.neighbors[i]);
            }
        }
    }
}
