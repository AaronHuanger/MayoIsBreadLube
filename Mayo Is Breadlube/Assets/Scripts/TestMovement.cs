using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING: This script is SUPER liable to changes, its a bunch of shit that I made for testing so alot of it is not optimal or smart in the slightest, its only here so I cant test
// The algorithm.
// If you do have questions about some of the functions used here, ask I guess.


public class TestMovement : MonoBehaviour
{
    private PathFinding pathFinding;
    private PathNode test;
    private BoundsInt bounds;
    private void Start()
    {
        pathFinding = GetComponent<PathFinding>();
        bounds = GetComponent<LayerControl>().bounds; 
    }
       
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            GetXY(mousePosition, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0,0, x, y);
            if(path!=null) 
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 test = new Vector3(path[i].x, path[i].y);
                    Debug.DrawLine(new Vector3(path[i].isometricCoordinates.x, path[i].isometricCoordinates.y + 0.25f) , new Vector3(path[i+1].isometricCoordinates.x, path[i+1].isometricCoordinates.y + 0.25f), Color.green);
                }
            }
        }
        if(Input.GetMouseButton(1))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
        }
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
   
    // Function works to return the location in context with our array, rather than taking the array and getting the position in context of the game worlds
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionwithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionwithZ()
    {
        return GetMouseWorldPositionwithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionwithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionwithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionwithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
