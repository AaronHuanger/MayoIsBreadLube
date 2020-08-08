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
    // Start needs to collect a couple things. Sometime in the future ill try and reduce these file accesses but for now this is what were using.
    // We need tilemap to change the color of walkable tiles, pathfinding for the actual path, and transform to move the character. 
    // We have a state which is set to normal, which tells our code that its primed to take in input.
    private void Start()
    {
        Transform bodyTransform = GetComponent<Transform>();
        pathFinding = GetComponentInParent<PathFinding>();
        tiles = GetComponentInParent<Tilemap>();
        originalColor = tiles.GetColor(new Vector3Int(0,0,0));
        state = State.Normal;
    }


   
    // Move is always called in update, however if you look at the code, whether it does anything or not depends on whether the pathlist actually has tiles to walk through. 
    // If our state is normal, then were primed to take in input. From there we update the tiles we can move to, and wait for input. If we click a tile, then all we do is simply check if the 
    //mouse position is within bounds and then if its within walking distance. If it is, we set our state to waiting (as in waiting to finish walking), and send our mouse world position
    //to SetTargetPosition to send into the pathfinding algorithm. Walk then takes the path given to us by SetTargetPosition and executes the path until the list is empty again. 
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
                    //Debug.Log("Mouse/Array Position: " + x + " , " + y);
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



    // To update what is and isnt within distance we deal exclusively with the pathfinding array (no world position transformations) aside from taking our mouse position and making it
    // an xy position in the pathnode array. We offset by our movement distance as normal and need to check a couple things. First we need to test whether there is a node there. 
    // IF there is we need to know whether we can even walk on it, and then we check if there can even be a path getting to that node. If there is a path then we check if its total
    // length is less then our movement distance, and then FINALLY we can color it and mark it if it is. Everything else is deemed not within distance and is flagged appropirately. 
     private void UpdateMovePosition()
    {
        int unitX = 0;
        int unitY = 0;
        Vector3Int tilePos;
        pathFinding.GetXY(GetPosition(), out unitX, out unitY);
        cleanTiles();

        for(int x = unitX - moveDistance; x <= unitX + moveDistance; x++)
        {
            for(int y = unitY - moveDistance; y <= unitY + moveDistance; y++)
            {
                if(pathFinding.GetNode(x,y) == null)
                    continue;
                if(pathFinding.isWalkable(x,y))
                {
                    // We know we dont have a path if the list we get from our current position to some supposed position turns up null. 
                    if(pathFinding.FindPath(unitX, unitY, x, y) != null)
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
    // Really all were doing here is getting our path list from our path finding algorithm. 
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

    //turns our tiles back to its original color, and resets whether the tile is within distance of us or not.
    private void cleanTiles()
    {
        // Our tile array doesnt really need to be manipulated in the same way it does with path finding i.e. transforming array positions into world positions. 
        // The only thing to really watch out for is that the tile position is in whole integers and functions almost like a cartesian plane. This means (-1,3) is totally valid
        // even though such a number cant be reacher with a normal for loop. The solution is just still using our same loop but offsetting what (0,0) is by adding the min bounds
        // given in layer.
        Vector3Int tilePos;
        for(int x = 0; x < pathFinding.grid.GetLength(0); ++x)
            for(int y = 0; y < pathFinding.grid.GetLength(1); ++y)
            {
                pathFinding.GetNode(x,y).withinDistance = false;
                tilePos = pathFinding.TilePosition(x,y); 
                tiles.SetColor(tilePos, originalColor);
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
