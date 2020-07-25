using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShiftControl : MonoBehaviour
{
    private Transform startingLayer;
    public Transform grid;
    public Transform[] gridMaps; //stores the tile maps of the grid object 
    int layerNum;
    public GameObject[] players;
    public GameObject startingPlayer;
    public Color outline = UnityEngine.Color.red;

    int playerNum;
    
    //Controls: mouse scrollbar --> shifts layesr
    //          shift + mouse scroll --> shifts player
    void Start()
    {   
        gridStart();
        playerStart();
    }

    void Update(){
        layerSwitch();
        playerSwitch();
    }

    void errorCheck(){
        /*if(!startingLayer.CompareTag("Layer")){
            Debug.LogError("A Layer was not placed in startingLayer");
        }*/
        if(!startingPlayer.CompareTag("Player")){
            Debug.LogError("A player was not placed in startingLayer");
        }
    }

    void gridStart(){
        //stores tile maps into an array for later access. 
        int numChild = grid.childCount;
        gridMaps = new Transform[numChild];
        for(int i = 0; i < numChild; i++){
            gridMaps[i] = grid.GetChild(i);
            gridMaps[i].GetComponent<TilemapRenderer>().enabled = false;
            hideChildren(gridMaps[i]);
        }
        layerNum = getLayerNum(startingPlayer.transform.parent);
        gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
        revealChildren(gridMaps[layerNum]);
    }

    void playerStart(){
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++){
            if(players[i] == startingPlayer){
                playerNum = i;
                break;
            }
        }
        startingPlayer.gameObject.GetComponent<SpriteRenderer>().color = outline;
    }
    void layerSwitch(){
        if (Input.mouseScrollDelta.y > 0 && (layerNum+1 < grid.childCount)){ // move layer up
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            gridMaps[layerNum].GetComponent<PathFinding>().enabled = false;
            gridMaps[layerNum].GetComponent<TestMovement>().enabled = false;
            hideChildren(gridMaps[layerNum]);
            layerNum++;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
            gridMaps[layerNum].GetComponent<PathFinding>().enabled = true;
            gridMaps[layerNum].GetComponent<TestMovement>().enabled = true;
            revealChildren(gridMaps[layerNum]);
        }else if(Input.mouseScrollDelta.y < 0 && (layerNum-1 >= 0)){ //move layer down 
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            gridMaps[layerNum].GetComponent<PathFinding>().enabled = false;
            gridMaps[layerNum].GetComponent<TestMovement>().enabled = false;
            hideChildren(gridMaps[layerNum]);
            layerNum--;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
            gridMaps[layerNum].GetComponent<PathFinding>().enabled = true;
            gridMaps[layerNum].GetComponent<TestMovement>().enabled = true;
            revealChildren(gridMaps[layerNum]);
        }
    }

    void playerSwitch(){
        if (Input.GetKeyDown("e") && (playerNum+1 < players.Length)){ // move layer up
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            hideChildren(gridMaps[layerNum]);
            players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
            playerNum++;
            layerNum = getLayerNum(players[playerNum].transform.parent);
            players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
            revealChildren(gridMaps[layerNum]);
        }else if(Input.GetKeyDown("q") && (playerNum-1 >= 0)){ //move layer down 
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = false;
            hideChildren(gridMaps[layerNum]);
            players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
            playerNum--;
            layerNum = getLayerNum(players[playerNum].transform.parent);
            players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            gridMaps[layerNum].GetComponent<TilemapRenderer>().enabled = true;
            revealChildren(gridMaps[layerNum]);
        }
    }
    int getLayerNum(Transform layer){
        for(int i = 0; i < grid.childCount; i++){
            if(gridMaps[i] == layer){
                return i;
            }
        }
        Debug.LogError("Layer doesn't exist. Failed to return layerindex");
        return -1; // crashes program. 
    }

    void hideChildren(Transform layer){
        foreach(Transform child in layer){
            child.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
    void revealChildren(Transform layer){
        foreach(Transform child in layer){
            child.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
