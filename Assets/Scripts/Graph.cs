using System.Collections.Generic;
using UnityEngine;

/*
 * Purpose of Graph class is to translate 1's and 0's 
 * from MapData.cs to an array of nodes
 */

public class Graph : MonoBehaviour {
    public Node [,] nodes;
    
    public List<Node> walls = new List<Node>();

    int[,] mapData;
    public int mapWidth = -1;
    public int mapHeight = -1;

    // keep track of the possible directions to move (horizontal, vertical, diagonal)
    public static readonly Vector2[] allDirections = {
        new Vector2(0f, 1f), 
        new Vector2(1f, 1f), 
        new Vector2(1f, 0f),
        new Vector2(1f, -1f),
        new Vector2(0f, -1f),
        new Vector2(-1f, -1f),
        new Vector2(-1f, 0f),
        new Vector2(-1f, 1f)
    };

    public void init(int[,] mapData) {
        this.mapData = mapData;
        mapWidth = mapData.GetLength(0);
        mapHeight = mapData.GetLength(1);
        nodes = new Node[mapWidth, mapHeight];


        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                NodeType nodeType = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, nodeType);
                nodes[x, y] = newNode;
                newNode.position = new Vector3(x, 0, y);
                if(nodeType == NodeType.Blocked) {
                    walls.Add(newNode);
                }
            }
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                // if node there is not blocked
                if (nodes[x, y].nodeType != NodeType.Blocked) {
                    nodes[x, y].neighbors = getNeighbors(x, y, nodes);
                }
            }
        }
    }

    public List<Node> getAllOpenNodes() {
        List<Node> openNodes = new List<Node>();
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (nodes[x, y].nodeType == NodeType.Open) {
                    openNodes.Add(nodes[x, y]);
                }
            }
        }
        return openNodes;
    }

    public bool isInRange(int x, int y) {
        return (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight);
    }

    List<Node> getNeighbors(int x, int y, Node[,] nodeArray) {
        List<Node> neighborNodes = new List<Node>();
        //Debug.Log("Current Node (" + nodeArray[x, y].position.x + ", " + nodeArray[x, y].position.z + ")");
        foreach (Vector2 d in allDirections) {
            int newX = x + (int)d.x; // new x for the direction we are looking in
            int newY = y + (int)d.y; // new y for the direction we are looking in

            if (isInRange(newX, newY) && 
                nodeArray[newX, newY] != null && 
                nodeArray[newX, newY].nodeType == NodeType.Open) {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
        //Debug.Log("Neighbor Count: " + neighborNodes.Count);

        return neighborNodes;
    }
}
