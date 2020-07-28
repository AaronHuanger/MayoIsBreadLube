using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    private int currentPathIndex;
    private PathFinding pathFinding;
    private List<Vector3> pathList = null;

    void Start()
    {
        Transform bodyTransform = GetComponent<Transform>();
        pathFinding = GetComponentInParent<PathFinding>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetMouseButton(0))
        {
            SetTargetPosition(GetMouseWorldPosition());
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
            Vector3 targetPosition = pathList[currentPathIndex];
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
