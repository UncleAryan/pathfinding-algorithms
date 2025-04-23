using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class GraphView : MonoBehaviour {
    public GameObject nodeViewPrefab;
    public Color openColor = Color.white;
    public Color blockedColor = Color.black;
    public NodeView[,] nodeViews;

    public void init(Graph graph) {
        if (graph == null) {
            Debug.LogWarning("GraphView error: No graph to initialize");
            return;
        }
        nodeViews = new NodeView[graph.mapWidth, graph.mapHeight];

        // go through each node in the nodes array in Graph class
        foreach(Node node in graph.nodes) {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null) {
                nodeView.init(node);
                nodeViews[node.xIndex, node.yIndex] = nodeView;
                if (node.nodeType == NodeType.Blocked) {
                    nodeView.colorNode(blockedColor);
                } else {
                    nodeView.colorNode(openColor);
                }
            }
        }
    }

    public void colorNodes(List<Node> nodes, Color color) {
        foreach (Node node in nodes) {
            if(node != null) {
                NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
                if (nodeView != null) {
                    nodeView.colorNode(color);
                }
            }
        }
    }
}
