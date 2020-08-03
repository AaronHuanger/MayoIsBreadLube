using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CharacterMovement : MonoBehaviour
{
    public float speed;
    public int moveDistance;
    private int currentPathIndex;
    private PathFinding pathFinding;
    private List<Vector3> pathList = null;
    private Tilemap tiles;
    private Color originalColor;

    private State state;
    private enum State
    {
        Normal,
        Waiting
    }
    private void Start()
    {
        Transform bodyTransform = GetComponent<Transform>();
        pathFinding = GetComponentInParent<PathFinding>();
        tiles = GetComponentInParent<Tilemap>();
        originalColor = tiles.GetColor(new Vector3Int(0,0,0));
        state = State.Normal;
    }

    // First in order to be able to display our possible moves were gonna need to check within a radius of our unit before hand and use our path finding to see if the move is valid.
    // since we have the pathfinding component already we can use our GetXY function to help sort out where we can move before hand.
   
    // Update is called once per frame
    void Update()
    {
        Move();
        switch(state)
        {
            case State.Normal:
                if(Input.GetMouseButton(0))
                {
                    UpdateMovePosition();
                    pathFinding.GetXY(GetMouseWorldPosition(), out int x, out int y);
                    Debug.Log("Mouse/Array Position: " + x + " , " + y);
                    if(!pathFinding.outOfBounds(x,y)) 
                    {
                        if(pathFinding.GetNode(x,y).withinDistance)
                        {   
                            state = State.Waiting;
                            SetTargetPosition(GetMouseWorldPosition());
                        }
                    }
                }
                UpdateMovePosition();
                break;
            case State.Waiting:
                break;
        }
    }


     private void UpdateMovePosition()
    {
        int unitX = 0;
        int unitY = 0;
        Vector3Int tilePos;
        pathFinding.GetXY(GetPosition(), out unitX, out unitY);
        for(int x = 0; x < pathFinding.grid.GetLength(0); ++x)
            for(int y = 0; y < pathFinding.grid.GetLength(1); ++y)
            {
                pathFinding.GetNode(x,y).withinDistance = false;
                tilePos = pathFinding.TilePosition(x,y); 
                tiles.SetColor(tilePos, originalColor);
            }
        for(int x = unitX - moveDistance; x <= unitX + moveDistance; x++)
        {
            for(int y = unitY - moveDistance; y <= unitY + moveDistance; y++)
            {
                if(pathFinding.GetNode(x,y) == null)
                    continue;
                if(pathFinding.isWalkable(x,y))
                {
                    if(pathFinding.hasPath(unitX, unitY, x, y))
                    {
                        if(pathFinding.FindPath(unitX, unitY, x, y).Count - 1 <= moveDistance)
                        {
                           // Debug.Log(pathFinding.FindPath(unitX, unitY, x, y).Count);
                           // Debug.Log("Is within distance: (" + x + " , " + y + ")");
                            pathFinding.GetNode(x,y).withinDistance = true;
                            tilePos = pathFinding.TilePosition(x,y);   
                            tiles.SetColor(tilePos, Color.green);
                        }
                        else
                        {
                           // Debug.Log(pathFinding.FindPath(unitX, unitY, x, y).Count);
                           // Debug.Log("Not within distance 1: (" + x + " , " + y + ")");
                            pathFinding.GetNode(x,y).withinDistance = false;
                            
                        }
                    }
                }
                else
                {
                    // Debug.Log(pathFinding.FindPath(unitX, unitY, x, y).Count);
                    // Debug.Log("Not within distance 2: (" + x + " , " + y + ")");
                    pathFinding.GetNode(x,y).withinDistance = false;
                    
                }
            }
        }
    }
    void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathList = pathFinding.FindPath(GetPosition(), targetPosition);

        if(pathList != null && pathList.Count > 1)
        {
            pathList.RemoveAt(0);
        }
    }
    void Move()
    {
        if(pathList != null)
        {
            // We collect the next node/tile on the list to walk to.
            Vector3 targetPosition = pathList[currentPathIndex]; 
            // The distance from our position to the next position has to be atleast 0.02 units for the move to be valid. 
            // If it is we just use a normal calculation to update its position, and if it isnt then were at the target node, in which case we increase our path index.
            // Should our path index be equal to our path list count then were done with the list.
            if(Vector3.Distance(transform.position, targetPosition) > 0.02f) 
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDirection * speed * Time.deltaTime;
            }
            else 
            {
                currentPathIndex++;
                if(currentPathIndex >= pathList.Count)
                {
                    pathList = null;
                    state = State.Normal;
                }
            }
        }
    }

    // Just returns the current position. Really for the sake of making things look neat.
    public Vector3 GetPosition()
    {
        return transform.position; 
    }


    // Function for getting the mouse world position. 
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
