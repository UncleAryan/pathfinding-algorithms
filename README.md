# Pathfinding Algorithms

- Pathfinding Algorithms implemented in Unity 3D. The framework is a connected unweighted graph of nodes (see node.cs).

- **Implemented Algorithms:**  
  - **A*** (A-Star)
  - **Greedy Best-First Search (GBFS)**
  - **Dijkstra's Algorithm (Shortest Path)**  
  - **Breadth-First Search (BFS)**
  - **Depth-First Search (DFS)**
 
- **How To Use:**
  - Start a blank Unity 3D Project
  - Download the repository
  - Replace your "Assets" folder with the one found in this repository
  - Import TextMesh Pro (TMP)
  - Set up an empty GameObject for every script in the "Scripts" folder that has a monobehaviour
  - Attach the scripts with monobehaviour to their respective GameObjects
  - Use the GameController script to change the algorithm, how fast the animation runs, and the start and goal node.
  - When working with algorithms that require heuristics, please use the Heuristics script to change which distancing is applied.
