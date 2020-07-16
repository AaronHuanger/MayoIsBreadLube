using UnityEngine;
using UnityEngine.Tilemaps;

public class GridControl : MonoBehaviour
{
    public TileBase[,] gridRepresentation; // Use a 2d array to store our grid represntation. 
    public TileBase testTile; // Test tile to be put into our array.
    int columns, rows, horizontal, vertical; // horizontal and vertical store our screens size, which is then used to get our columns and rows. These will be changed later
                                             // most likely to fit whatever size map we may need, but for now I just want something to fit neatly on screen.
    void Start()
    {
        horizontal = (int) Camera.main.orthographicSize; // Get the size of our screen from our camera. 
        vertical = horizontal * (Screen.width/Screen.height) ; // For our vertical we just take our horizontal and multiply by our screens ratio.
        columns = horizontal * 2; // The horizontal and vertical return us half of the size we actually want, so multiply by two to get the full number of columns
        rows = vertical * 2;
        
        gridRepresentation = new TileBase[columns,rows];

        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                gridRepresentation[i,j] = testTile;
                displayTile(i, j, gridRepresentation[i,j]);
            }
        }
    }

    private void displayTile(int x, int y, TileBase myTile)
    {
        Tilemap tileMap = GetComponent<Tilemap>();
        Vector3Int tilePos = new Vector3Int(x - (horizontal), y -(vertical),0); // Since we start at (0,0) we need to off set our actual x by the size of our screen. 
        tileMap.SetTile(tilePos, myTile); // Use the set tile method to place our tile neatly. 
    }

}
