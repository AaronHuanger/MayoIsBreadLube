using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    private PathFinding pathFinding;
    private PathNode test;
    private BoundsInt bounds;
    private void Start()
    {
        bounds = GetComponent<LayerControl>().bounds;
        pathFinding = GetComponent<PathFinding>();
        
    }

    private void Update()
    {
            List<PathNode> path = pathFinding.FindPath(0,0, 4, 4);
            if(path!=null) 
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 test = new Vector3(path[i].x, path[i].y);
                   // Debug.Log("(" + test.x + "," + test.y + ")");
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y + 0.25f) , new Vector3(path[i+1].x ,path[i+1].y + 0.25f), Color.green, 1000f);
                }
            }
    }
   /* private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();

            List<PathNode> path = pathFinding.FindPath(0,0, (int)mousePosition.x + bounds.xMin, (int)mousePosition.y + bounds.yMin);
            if(path!=null) 
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 test = new Vector3(path[i].x, path[i].y);
                   // Debug.Log("(" + test.x + "," + test.y + ")");
                    Debug.DrawLine(new Vector3(path[i].x - bounds.xMin, path[i].y - bounds.yMin + 0.25f) , new Vector3(path[i+1].x - bounds.xMin ,path[i+1].y  - bounds.yMin + 0.25f), Color.green);
                }
            }
        }
        if(Input.GetMouseButton(1))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
        }
    }*/
    
    /*public static Vector3 GetMouseWorldPosition()
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
    }*/
}
