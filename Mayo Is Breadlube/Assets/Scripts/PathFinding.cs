using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING: Lot of comments because id rather you read this, or this article https://www.raywenderlich.com/3016-introduction-to-a-pathfinding than me explain from the ground up in call.

public class PathFinding : MonoBehaviour
{
    // Because working with decimals is wonky in our algorithm instead of costing one point to move a tile we scale it up so it cost 10.
    // However since we dont have diagonal  movement, which usually cost 1.4 or 14 if we scale, we dont techinally need this.
    private const int MOVE_STRAIGHT_COST = 10;                            
    public PathNode[,] grid; 
    // We need two list for this algorithm, and openlist for all the nodes we havent checked and a closed list for the ones we have
    private List<PathNode> openList; 
    private List<PathNode> closedList;
    private BoundsInt bounds;
    
    
    // We first need to get our path node array, which will have the array with the appropriate bounds. 
    void Start() 
    {
        grid = GetComponent<LayerControl>().pathNodes;  
        bounds = GetComponent<LayerControl>().bounds;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        GetXY(startWorldPosition, out int startX, out int startY);
        GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if(path == null)
        {
            return null;
        }
        else 
        {
             List<Vector3> vectorPath = new List<Vector3>();
             foreach(PathNode pathNode in path)
             {
                 vectorPath.Add(new Vector3(pathNode.isometricCoordinates.x, pathNode.isometricCoordinates.y + 0.25f));
             }
             return vectorPath; 
        }

    }
    
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        if(!hasPath(startX, startY, endX, endY))
        {
            return null;
        }
        // Our beginning node is of course whatever tile we begin at, Same with our end node.
        PathNode startNode = grid[startX, startY];
        PathNode endNode = grid[endX, endY]; 
        
        // Our open list starts with the start node, and the algorithm will work from this starting point.
         // Our closed list is of course empty at first.
        openList = new List<PathNode> { startNode }; 
        closedList = new List<PathNode>();

        // Even though we may have already created an array of pathnodes with appropriate x,y values in our layer control
        // calculations in the h, f, and g cost change for a given start and end node, so we need to reset the board appropriately.
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
       
        // The g cost is the cost it takes to go from the starting node to some other node following the path of adjacent nodes(or tiles). So 
        // from the starting node, a node 3 nodes/tiles to the right of the starting node would have a g cost of 30. 
        startNode.gCost = 0;   
        // Our h cost is pretty much the opposite, the cost of getting from some node to the end node.
        startNode.hCost = CalculateDistance(startNode, endNode); 
        // The f cost is simply the sum of these two cost.
        startNode.CalculateFCost(); 

        // While our list has nodes to check, check those nodes for a possible path.
        while(openList.Count > 0) 
        {
            // We first look at our list of open nodes and get the one with the lowest f cost to check first
            // We remove it from our open list and put it in our close list since we are checking it.
            PathNode currentNode = GetLowestFCost(openList); 
            openList.Remove(currentNode); 
            closedList.Add(currentNode);

            // If our current node is our end node, then we have found our path, its easier to see why this is the case visually. 
            if(currentNode == endNode) 
            {
                //Reached the final node
                return CalculatePath(startNode, endNode);
            } 

            // For each neighbor node to the current node we do a couple things.
            // If our neighbor node has already been checked, just continue going through the neighbors list
            // If the node is unwalkable go ahead and add it to the close list and continue.
            // Other wise we do the last if check which is hard to explain without visuals, so if you have question ask me directly or implement the algorithm for yourself to find out!
            foreach(PathNode neighbourNode in GetNeighbourList(currentNode)) 
            {
                if(closedList.Contains(neighbourNode)) continue; 
                if(!currentNode.walkableTile) 
                { 
                    closedList.Add(currentNode);
                    continue;
                }

                int newMovementCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
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

    // This will be optomized later with a binary tree, but right now getting the neighbours is straightforward.
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
        if(!outOfBounds(x,y))
            return grid[x,y];
        else 
            return null;
    }

    public bool outOfBounds(int x, int y)
    {
        if(x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0)
            return true;
        else
            return false;
    }

    // Since every node has a pointer to the node it came from, to get the path we simply 
    // start from our end node, go all the way back to our start node, and then reverse the list for our correct path.
    private List<PathNode> CalculatePath(PathNode startNode, PathNode endNode) 
    {                                                                          
        List<PathNode> path =  new List<PathNode>();                          
        PathNode currentNode = endNode;
        
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }
        path.Add(currentNode);
        path.Reverse();
        return path;
    }

    // Basic algebra for distances between two points.
    private int CalculateDistance(PathNode a, PathNode b)
    {
        return MOVE_STRAIGHT_COST * Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); 
    }

     // Simple using a loop to find the lowest value.
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

    public void GetXY (Vector3 worldPosition, out int x, out int y) 
    {
        float tempx;
        float calcX, calcY;
        calcX = (bounds.xMin - bounds.yMin) * 0.5f;
        calcY = (bounds.xMin + bounds.yMin) * 0.25f;
      
        Vector3 originPosition = new Vector3(calcX, calcY);
        worldPosition = (worldPosition - originPosition) / 0.5f;
        tempx = worldPosition.x;
        worldPosition.x = (2.0f * worldPosition.y + worldPosition.x) * 0.5f;
        worldPosition.y = (2.0f * worldPosition.y - tempx) * 0.5f;
        
        x = Mathf.FloorToInt(worldPosition.x);
        y = Mathf.FloorToInt(worldPosition.y);
    }  

    public bool isWalkable(int x, int y)
    {
        if(outOfBounds(x,y))
            return false;
        if(grid[x,y].walkableTile)
            return true;
        else 
         return false;
    }

    public bool hasPath(int startX, int startY, int endX, int endY)
    {
        if(endX >= grid.GetLength(0) || endX < 0 || endY >= grid.GetLength(1) || endY < 0)
        {
            return false;
        }
        else 
            return true;
    }

    public Vector3 WorldPosition(int x, int y) 
    {
        float calcX, calcY;
        calcX = (bounds.xMin - bounds.yMin) * 0.5f;
        calcY = (bounds.xMin + bounds.yMin) * 0.25f;
        Vector3 rotationCalculation;
        
        rotationCalculation =  new Vector3 ((x -y), (float)(y+x)/2);  
        return rotationCalculation * 0.5f + new Vector3(calcX, calcY); 
    } 

    public Vector3Int TilePosition(int x, int y)
    {
        Vector3Int holder;
        holder = new Vector3Int(x  + bounds.xMin, y + bounds.yMin, 0);
        return holder;
    }     
}
