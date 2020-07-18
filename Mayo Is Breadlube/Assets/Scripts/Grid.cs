using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// Grid class 
public class Grid<T> 
{
    public event EventHandler<OnGridObjectChangeEventArgs> OnGridObjectChange;
    public class OnGridObjectChangeEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private T[,] gridRepresentation; // Array of whatever it is we want, could be as simple as bools or as complicated as our own data types. 
 
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> defaultObject) // constructor for our gird class. Given the fact that we dont know
    {                                                                                                        // what T is, we can pass a function that gives us the default value of that type.
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition; // The origin of our grid is the left most corner of the square/diamond. 

        gridRepresentation = new T[width, height];

        for(int x = 0; x < gridRepresentation.GetLength(0); x++) // fill our array with the default object. 
        {
            for(int y = 0; y < gridRepresentation.GetLength(1); y++)
            {
               gridRepresentation[x,y] = defaultObject(this, x, y); // fill our grid with the default object for whatever type we are using.
            } // This way of doing things should probably be changed, we should just keep the default func as Func<T>, and instead have the T subscribe to an event in order to by pass all these refs.
        }

        bool showDebug = true;
        if(showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            for(int x = 0; x < gridRepresentation.GetLength(0); x++)
            {
                for(int y = 0; y < gridRepresentation.GetLength(1); y++)
                {
                    debugTextArray[x,y] = CreateWorldText(x.ToString() + "," +  y.ToString(), null, WorldPosition(x,y) + new Vector3(cellSize, 0), 15, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(WorldPosition(x,y), WorldPosition(x, y+1),Color.white,100f); // by offsetting it by half of the cell size 
                    Debug.DrawLine(WorldPosition(x,y), WorldPosition(x+1,y),Color.white,100f);
                }
            }
            Debug.DrawLine(WorldPosition(0,height), WorldPosition(width,height),Color.white,100f);
            Debug.DrawLine(WorldPosition(width,0), WorldPosition(width,height),Color.white,100f);

            //OnGridObjectChange +=  (object sender, OnGridObjectChangeEventArgs eventArgs) =>  {} here is the function that would handle our event everytime we call it if we need one.
        }
    }
    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }

    public float getCellSize()
    {
        return cellSize;
    }

    public void TriggerObjectChange(int x, int y) // Function to use for updating the grid upon changes occuring.
    {
        if (OnGridObjectChange != null) OnGridObjectChange(this, new OnGridObjectChangeEventArgs { x = x, y = y});  
    }


    public void SetObject(int x, int y, T myObject) // Get set the object at a specific entry in our array.
    {
        if(x >=0 && y >=0 && x <= width && y <= height)
        {
            gridRepresentation[x,y] = myObject;
            if(OnGridObjectChange != null) OnGridObjectChange(this, new OnGridObjectChangeEventArgs{x = x, y = y });
        }
    }

    public void SetObject(Vector3 worldPosition, T myObject) // Set an object using position data, uses the get cartesian function to determine what part of the array the object is apart of.
    {
        int x, y;
        GetCartesian(worldPosition, out x, out y);
        SetObject(x,y, myObject);
    }      

    public T GetObject(int x, int y) // get the object of a specific entry of the array.
    {
        if(x >=0 && y >=0 && x <= width && y <= height)
        {
            return gridRepresentation[x,y];
        }
        else return default(T);
    }

    public T GetObject(Vector3 worldPosition) // get an onbject based on its location. 
    {
        int x,y;
        GetCartesian(worldPosition, out x, out y);
        return GetObject(x,y);
    }

    private Vector3 WorldPosition(int x, int y) // positions is really just the x and y multuplied by the cell size
    {
        Vector3 rotationCalculation;
         rotationCalculation =  new Vector3 ((x + y), (float)(y-x)/2);  // Since we are working in isometric plane, we simply take our cartesian coordinates and convert them to iso.
        return rotationCalculation * cellSize + originPosition; // We also need to account for cell size, which is done here. Since our origin can be changed we simply add
    }                                                           // this offset to our vector to return the correct position.

    private void GetCartesian(Vector3 worldPosition, out int x, out int y) // Function works to return the location in context with our array, rather than taking the array and getting the position in context of the game worlds
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x/cellSize); // In order to get the cartesian with a given origin we subtract our position from our origin. 
        y = Mathf.FloorToInt((worldPosition - originPosition).y/cellSize);

    }   

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 5, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000) 
    {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
        
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) 
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        textMesh.characterSize = 0.1f; 
        return textMesh;
    }
}
