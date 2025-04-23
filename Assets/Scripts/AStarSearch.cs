using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStarSearch : MonoBehaviour {
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
        startNode.f = 0;
        startNode.g = 0;
        goalNode = goal;

        exploredNodes = new List<Node>();
        pathNodes = new List<Node>();
        frontierNodes = new List<Node>();

        for (int y = 0; y < graph.mapHeight; y++) {
            for (int x = 0; x < graph.mapWidth; x++) {
                graph.nodes[x, y].reset();
                graph.nodes[x, y].g = int.MaxValue;
                graph.nodes[x, y].h = graph.nodes[x, y].distance;
                graph.nodes[x, y].f = int.MaxValue;
            }
        }

        startNode.g = 0;
        startNode.h = startNode.distance;
        startNode.f = startNode.g + startNode.h;
        frontierNodes.Add(startNode);

        showColors();

        isComplete = false;
        iterations = 0;
    }

    public IEnumerator searchRoutine(float timeStep = 0.1f) {
        while (!isComplete) {
            if (frontierNodes.Count > 0) {
                iterations++;

                Node currentNode = getLowestNode(frontierNodes); // lowest f cost node
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

    private void expandFrontier(Node node) {
        foreach (Node neighbor in node.neighbors) {
            if (!exploredNodes.Contains(neighbor)) {
                int tentativeGCost = node.g + 1;
                if (tentativeGCost < neighbor.g) {
                    neighbor.g = tentativeGCost;
                    neighbor.h = neighbor.distance;
                    neighbor.f = neighbor.g + neighbor.h;

                    if (!exploredNodes.Contains(neighbor) &&
                        !frontierNodes.Contains(neighbor)) {
                        neighbor.previous = node;
                        frontierNodes.Add(neighbor);
                    }
                }
            }
        }
    }

    private Node getLowestNode(List<Node> nodes) {
        Node lowest = nodes[0];

        for(int i = 0; i < nodes.Count; i++) {
            if (nodes[i].f < lowest.f || (nodes[i].f == lowest.f && nodes[i].h < lowest.h)) {
                lowest = nodes[i];
            }
        }

        return lowest;
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

}
