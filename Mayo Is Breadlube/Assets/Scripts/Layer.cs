using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Layer : MonoBehaviour
{
    public MyTileData[,] tiles; 
    public TileBase testTile;
    public float fadeRate = 1f;
    private Tilemap curTileMap;
    private BoundsInt bounds;

    // Start is called before the first frame update
    void Awake()
    {
        curTileMap = transform.GetComponent<Tilemap>(); // gets the tilemap that this script is attached to 
        bounds = getTightBounds(curTileMap);
        makeBoard();
        //Note - the offset value of the x and y is the minValues of the bound of the x and y's
        // add the minBounds when you want to go from tileMap to array and subtract the minBound from the index of the array when going from array to tilemap positions.
        //ex: bounds.xMin and bounds.yMin. 
        tiles = new MyTileData[Mathf.Abs(bounds.xMax - bounds.xMin)+1, Mathf.Abs(bounds.yMax - bounds.yMin)+1]; // creates an array of tiles based on the map
    }

    public BoundsInt getBounds(){
        return bounds;
    }
    
    public void setBounds(BoundsInt newBound){
        bounds = newBound;
    }

    public static BoundsInt getTightBounds(Tilemap curTileMap){
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        Vector3Int tempVec;
        BoundsInt bounds = new BoundsInt();
        for(int i = curTileMap.cellBounds.xMin; i < curTileMap.cellBounds.xMax; i++){
            for(int j = curTileMap.cellBounds.yMin; j < curTileMap.cellBounds.yMax;j++){
                tempVec = new Vector3Int(i,j,0); // contains the position of the tilemap. 
                if(curTileMap.HasTile(tempVec)){ //check the position
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
    private void makeBoard(){ // makes the board at the spot except for existing tiles

        int xCount = bounds.xMin;
        int yCount = bounds.yMin;
        while(yCount <= bounds.yMax){
            Vector3Int tilePos = new Vector3Int(xCount, yCount, 0);
            if(!curTileMap.HasTile(tilePos)){
                //curTileMap.SetTileFlags(tilePos, TileFlags.None);
                curTileMap.SetTile(tilePos,testTile);
            }
            xCount++;
            if(xCount > bounds.xMax){
                xCount = bounds.xMin;
                yCount++;
            }
        }
    }

    public void changeColor(Color color){
        Vector3Int tempPos;

        int xCount = bounds.xMin;
        int yCount = bounds.yMin;

        while(yCount <= bounds.yMax){
            tempPos = new Vector3Int(xCount , yCount , 0);
            if(curTileMap.HasTile(tempPos)){
                curTileMap.SetTileFlags(tempPos, TileFlags.None);
                curTileMap.SetColor(tempPos, color);
            }
            //changes y position of the tilemap
            xCount++;
            if(xCount > bounds.xMax){
                xCount = bounds.xMin;
                yCount++;
            }
        }
    }

    public void changeOpacity(float alphaNum){
        Vector3Int tempPos;
        Color tempColor;
        int xCount = bounds.xMin;
        int yCount = bounds.yMin;

        while(yCount <= bounds.yMax){
            tempPos = new Vector3Int(xCount , yCount , 0);
            tempColor = transform.GetComponent<Tilemap>().GetColor(tempPos);
            tempColor.a = alphaNum;
            if(curTileMap.HasTile(tempPos)){
                curTileMap.SetTileFlags(tempPos, TileFlags.None);
                curTileMap.SetColor(tempPos, tempColor);
            }
            //changes y position of the tilemap
            xCount++;
            if(xCount > bounds.xMax){
                xCount = bounds.xMin;
                yCount++;
            }
        }
    }
    public void removeTileFlags(){

    }

    public IEnumerator fadeIn(){
        Color tempColor;
        for(float i = 0; i < 1; i += Time.deltaTime*fadeRate){
            //fadein every tile of the layer
            changeOpacity(i);
            //fadein every child of the layer
            foreach(Transform child in transform){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }
        //ensures that everything has an alpha value of 1 
        changeOpacity(1);

        foreach(Transform child in transform){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            child.gameObject.SetActive(true); //deactivates every child of the layer
        }
        //activates the layer
        transform.gameObject.SetActive(true);
    }
    public IEnumerator fadeOut(){
        Color tempColor;
        for(float i = 1; i > 0; i -= Time.deltaTime*fadeRate){
            //fades every tile of the layer
            changeOpacity(i);
            //fades every child of the layer
            foreach(Transform child in transform){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }

        //ensures that everything has an alpha value of 0 
        changeOpacity(0);

        foreach(Transform child in transform){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            //child.gameObject.SetActive(false); //deactivates every child of the layer
            //Note child objects are automatically deactivated 
        }
        //deactivates the layer afterwards so it doesn't interfere with other operations.
        transform.gameObject.SetActive(false);
    }
}
