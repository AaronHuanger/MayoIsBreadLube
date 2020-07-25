using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    public PathNode previousNode;
    public bool walkableTile;

    public PathNode( int x, int y)
    {  
        this.x = x;
        this.y = y;
        this.walkableTile = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    
}
