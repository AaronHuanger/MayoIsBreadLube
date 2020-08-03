using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    // Array poisitions
    public int x; 
    public int y;
    //Variables for path Finding calculations
    public int gCost;
    public int hCost;
    public int fCost;
    //Variables for turning coordinates from the cartesian x and y into isometric coordinates.
    public Vector3 isometricCoordinates;
    private float xCalc;
    private float yCalc;
    // Variable to hold the node that came before this node, used for path finding calculation.
    public PathNode previousNode;
    // Variable to set whether tile is walkable.
    public bool walkableTile = true;
    public bool withinDistance = false;


    public PathNode(int x, int y, BoundsInt bounds)
    {  
        this.x = x;
        this.y = y;
        this.walkableTile = true;
        // In order to calculate our isometric coordinates we add the "origin" of our grid which is our min bounds.
        // However these bounds are 1. In cartesian coordiantes and 2. not scaled to the size of our tile, which in this case is 0.5
        this.xCalc = (x + bounds.xMin) * 0.5f; 
        this.yCalc = (y + bounds.yMin) * 0.5f; 
        // With the scaling and offsets accounter for, we simply do the conversion.
        // It is as simple as X = CartX - CartY, Y = (CartX + CartY) \ 2 or in this case * 0.5
        isometricCoordinates = new Vector3(xCalc - yCalc, (xCalc + yCalc) * (0.5f));                                                       
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    
}
