using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
   // private const int MOVE_DIAGONAL_COST = 14;
    PathNode[,] grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    
    void Start()
    {
        grid = GetComponent<LayerControl>().pathNodes;  
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log("test");
        PathNode startNode = grid[startX, startY];
        PathNode endNode = grid[endX, endY];
        

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                
                PathNode pathNode = grid[x,y];
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }
        }
       
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCost(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                //Reached the final node
                return CalculatePath(startNode, endNode);
            } 

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode)) continue;
                if(!currentNode.walkableTile) 
                { 
                    closedList.Add(currentNode);
                    continue;
                }

                int newMovementCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
              //  Debug.Log("Currently in (" + currentNode.x  + " , " + currentNode.y + ")");
                if(newMovementCost < neighbourNode.gCost || !openList.Contains(neighbourNode))
                {
                    
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = newMovementCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                } 
                 
            }
        }
        //Out of nodes on the open list;
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        //Left
        if(currentNode.x - 1 >= 0)  neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
        //Right
        if(currentNode.x + 1 < grid.GetLength(0))  neighbourList.Add(GetNode(currentNode.x + 1 , currentNode.y));
        //Down
        if(currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        //Up
        if(currentNode.y + 1 < grid.GetLength(1)) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid[x,y];
    }

    private List<PathNode> CalculatePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path =  new List<PathNode>();
        PathNode currentNode = endNode;
        
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
      /*  int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;*/
        return MOVE_STRAIGHT_COST * Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private PathNode GetLowestFCost(List<PathNode> list)
    {
        PathNode loswestFCost = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if(list[i].fCost < loswestFCost.fCost || (list[i].fCost == loswestFCost.fCost && list[i].hCost < loswestFCost.hCost)) 
                loswestFCost = list[i];
        }
        return loswestFCost;
    }
}
