using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerShift : MonoBehaviour
{
    public Transform grid;
    public static Transform[] gridMaps; //stores the tile maps of the grid object 
    int layerNum;
    
    //store child objects of the grid into an array
    //"q" will move the layer "up" and "e" will move the layer "down"; 
    //aka q will make current layer invisible and make the next layer in the array visible.

    void Start()
    {
        //stores tile maps into an array for later access. 
        int numChild = grid.childCount;
        gridMaps = new Transform[numChild];
        for(int i = 0; i < numChild; i++){
            gridMaps[i] = grid.GetChild(i);
            gridMaps[i].GetComponent<TilemapRenderer>().enabled = false;
            //gridMaps[i].gameObject.SetActive(false);
            //gridMaps[i].GetComponent<Tilemap>().ClearAllTiles();
        }
        layerNum = gridMaps.Length/2;
        gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;

    }

    void Update(){
        switchCheck();
    }

    void switchCheck(){
        if (Input.GetKeyDown("q") && (layerNum+1 < grid.childCount)){ // move layer up
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            layerNum++;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
        }else if(Input.GetKeyDown("e") && (layerNum-1 >= 0)){ //move layer down 
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            layerNum--;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
        }
    }
}
