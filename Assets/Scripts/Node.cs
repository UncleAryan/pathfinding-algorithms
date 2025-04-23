using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
    Open = 1, Blocked = 0
}

public class Node {
    public NodeType nodeType = NodeType.Open;
    public int xIndex = -1;
    public int yIndex = -1;
    public Vector3 position;
    public List<Node> neighbors = new List<Node>();
    public Node previous = null;
    public int distance; // heuristics mapping
    public int dijkstraDistance; // pretty obvious bro
    public int f, g, h; // for A*

    public Node(int xIndex, int yIndex, NodeType nodeType) {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    public void reset() {
        previous = null;
    }
}

