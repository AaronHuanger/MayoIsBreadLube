using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerControl : MonoBehaviour
{
    public TileData[,] tiles; 
    public TileBase testTile;
    Tilemap curMap;
    BoundsInt bounds;
    int xCount;
    int yCount;
    // Start is called before the first frame update
    void Start()
    {
        curMap = GetComponent<Tilemap>(); // gets the tilemap that this script is attached to 

        bounds = getTightBounds(curMap);
        //curMap.size = bounds.size;
        //curMap.origin = new Vector3Int((bounds.xMax + bounds.xMin)/2 , (bounds.yMax + bounds.yMin)/2, 0); //adjusts the origin of the tilemap so that it is centered on the square.
        //curMap.ResizeBounds(); // resizes the bounds to fit the existing tiles. Resize based on size and origin of the tilemap
        //Note: no need to resize the bounds of the tilemap. Wasn't possible to do so anyways. 

        //xCount = bounds.xMin; // test variable for LateUpdate test
        //yCount = bounds.yMin; // test variable for LateUpdate test

        makeBoard();

        //Note - the offset value of the x and y is the minValues of the bound of the x and y's
        // add the minBounds when you want to go from tileMap to array and subtract the minBound from the index of the array when going from array to tilemap positions.
        //ex: bounds.xMin and bounds.yMin. 
        tiles = new TileData[bounds.size.x, bounds.size.y]; // creates an array of tiles based on the map
    }


    // a test function to help visual the filling ni of the board. 
    /*void LateUpdate() 
    {
        //if(Input.GetKeyDown("f")){
        if(Input.GetKey("f")){
            Vector3Int tilePos = new Vector3Int(xCount, yCount, 0);
            if(curMap.GetComponent<TilemapRenderer>().enabled){
                if(!curMap.HasTile(tilePos)){
                    curMap.SetTile(tilePos,testTile);
                }
            }
            xCount++;
            if(xCount > (tiles.GetLength(0) - Mathf.Abs(bounds.xMin))){
            //if(xCount > xMax){
                xCount = bounds.xMin;
                yCount++;
            }
            
            if(yCount > (tiles.GetLength(1) - Mathf.Abs(bounds.yMin))){
            //if(yCount > yMax){
                yCount = bounds.yMin;
            }
        }
    }*/

    public static BoundsInt getTightBounds(Tilemap curMap){
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        Vector3Int tempVec;
        BoundsInt bounds = new BoundsInt();
        for(int i = curMap.cellBounds.xMin; i < curMap.cellBounds.xMax; i++){
            for(int j = curMap.cellBounds.yMin; j < curMap.cellBounds.yMax;j++){
                tempVec = new Vector3Int(i,j,0); // contains the position of the tilemap. 
                if(curMap.HasTile(tempVec)){ //check the position
                    minX = Mathf.Min(minX, tempVec.x);
                    maxX = Mathf.Max(maxX, tempVec.x);
                    minY = Mathf.Min(minY, tempVec.y);
                    maxY = Mathf.Max(maxY, tempVec.y);
                }
            }
        }
        bounds.SetMinMax(new Vector3Int(minX, minY, 0), new Vector3Int(maxX, maxY, 1));
        return bounds;
    }

//make board function 
    public void makeBoard(){ // makes the board at the spot except for existing tiles

        /*Vector3Int tilePos;
        for(int i = bounds.yMin; i < bounds.yMax; i++){
            for(int j = bounds.xMin; j < bounds.xMax; j++){
                tilePos = new Vector3Int(j,i,0);
                if(!curMap.HasTile(tilePos)){
                    curMap.SetTile(tilePos,testTile);
                }
            }
        }*/

        xCount = bounds.xMin;
        yCount = bounds.yMin;
        while(yCount <= (bounds.size.y - Mathf.Abs(bounds.yMin))){
            Vector3Int tilePos = new Vector3Int(xCount, yCount, 0);
            if(!curMap.HasTile(tilePos)){
                curMap.SetTile(tilePos,testTile);
            }
            xCount++;
            if(xCount > (bounds.size.x - Mathf.Abs(bounds.xMin))){
                xCount = bounds.xMin;
                yCount++;
            }
        }
    }
}
