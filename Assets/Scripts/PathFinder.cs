using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates{ get{return startCoordinates;}}
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates{ get{return destinationCoordinates;}}

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier=new Queue<Node>(); 
    Dictionary<Vector2Int, Node> reached=new Dictionary<Vector2Int, Node>(); //visited in BFS

    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid=new Dictionary<Vector2Int, Node>();
    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager != null) {
            grid=gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath() {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates) {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    private void ExploreNeighbours()
    {
        List<Node> neighbours=new List<Node>();
        foreach (Vector2Int d in directions)
        {
            Vector2Int neighbourCoords = currentSearchNode.coordinates + d;

            if(grid.ContainsKey(neighbourCoords)) {
                neighbours.Add(grid[neighbourCoords]);
            }
        }

        foreach (Node n in neighbours) 
        {
            if(!reached.ContainsKey(n.coordinates) && n.isWalkable) {
                n.connectedTo = currentSearchNode;
                reached.Add(n.coordinates, n);
                // Debug.Log(n.coordinates);
                frontier.Enqueue(n);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates) {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while(frontier.Count>0 && isRunning) {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if(currentSearchNode.coordinates == destinationNode.coordinates) {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() {
        List<Node> path=new List<Node>();
        Node currNode=destinationNode;
        
        path.Add(currNode);
        currNode.isPath=true;

        while(currNode.connectedTo != null) {
            currNode = currNode.connectedTo;
            path.Add(currNode);
            currNode.isPath=true;
        }

        path.Reverse();
        
        //testing
        // foreach (var item in path)
        // {
            // Debug.Log(item.coordinates);
        // }

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates) {
        if(grid.ContainsKey(coordinates)) {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable=false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count<=1) {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyRecievers() {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
