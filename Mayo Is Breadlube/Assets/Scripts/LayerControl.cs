using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerControl : MonoBehaviour
{
    public MyTileData[,] tiles; 
    public TileBase testTile;
    Tilemap curMap;
    private BoundsInt bounds;

    // Start is called before the first frame update
    void Start()
    {
        curMap = GetComponent<Tilemap>(); // gets the tilemap that this script is attached to 

        bounds = getTightBounds(curMap);

        makeBoard();

        //Note - the offset value of the x and y is the minValues of the bound of the x and y's
        // add the minBounds when you want to go from tileMap to array and subtract the minBound from the index of the array when going from array to tilemap positions.
        //ex: bounds.xMin and bounds.yMin. 
        tiles = new MyTileData[bounds.size.x, bounds.size.y]; // creates an array of tiles based on the map
    }

    public BoundsInt getBounds(){
        return bounds;
    }
    public void setBounds(BoundsInt newBound){
        bounds = newBound;
    }
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

        int xCount = bounds.xMin;
        int yCount = bounds.yMin;
        while(yCount <= bounds.yMax){
            Vector3Int tilePos = new Vector3Int(xCount, yCount, 0);
            if(!curMap.HasTile(tilePos)){
                curMap.SetTile(tilePos,testTile);
            }
            xCount++;
            if(xCount > bounds.xMax){
                xCount = bounds.xMin;
                yCount++;
            }
        }
    }
}
