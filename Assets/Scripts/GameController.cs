using UnityEngine;

using UnityEngine;

/*
 * From there we can run other classes.
 */
public class GameController : MonoBehaviour {
    public MapData mapData;
    public Graph graph;
    public Heuristics heuristics;
    public PathFinder pathFinder;
    public DFS dfs;
    public GreedySearch greedySearch;
    public DijkstraSearch dijkstraSearch;
    public AStarSearch aStarSearch;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 4;
    public int goalY = 1;
    public float timeStep = 0.1f;

    public bool BFS = false;
    public bool DFS = false;
    public bool GreedySearch = false;
    public bool DijkstraSearch = false;
    public bool AStarSearch = false;

    void Start() {
        if (mapData != null && graph != null) {
            int[,] mapInstance = mapData.makeMap(); // making a 2d array based off the nodes (0, 1)
            graph.init(mapInstance); // convert above to array of nodes

            GraphView graphView = graph.GetComponent<GraphView>();
            if (graphView != null && heuristics != null) {
                heuristics.init(graph, graph.nodes[goalX, goalY]);
                graphView.init(graph);
            } else {
                Debug.Log("Error");
            }

            if (graph.isInRange(startX, startY) && graph.isInRange(goalX, goalY) && pathFinder != null) {
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];

                if (BFS) {
                    pathFinder.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(pathFinder.searchRoutine(timeStep));
                } else if (DFS) {
                    dfs.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(dfs.searchRoutine(timeStep));
                } else if (GreedySearch) {
                    greedySearch.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(greedySearch.searchRoutine(timeStep));
                } else if (GreedySearch) {
                    greedySearch.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(greedySearch.searchRoutine(timeStep));
                } else if (DijkstraSearch) {
                    dijkstraSearch.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(dijkstraSearch.searchRoutine(timeStep));
                } else if(AStarSearch) {
                    aStarSearch.init(graph, graphView, startNode, goalNode);
                    StartCoroutine(aStarSearch.searchRoutine(timeStep));
                }
            } else {
                Debug.Log("out of bounds");
            }
        }


    }

    
}
