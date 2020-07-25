using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShiftControl : MonoBehaviour
{
    public Transform grid;
    public Transform[] gridMaps; //stores the tile maps of the grid object 
    private Transform startingLayer;
    int layerNum;
        public GameObject[] players;
    public GameObject startingPlayer;
    int playerNum;
    public Color outline = UnityEngine.Color.red;
    public float fadeRate = 0.5f;  
    
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
            IEnumerator fadeOutCR = fadeOut(gridMaps[i]);
            StartCoroutine(fadeOutCR);
        }
        layerNum = getLayerNum(startingPlayer.transform.parent);
        IEnumerator fadeInCR = fadeIn(gridMaps[layerNum]);
        StartCoroutine(fadeInCR);
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
            IEnumerator fadeOutCR = fadeOut(gridMaps[layerNum]);
            StartCoroutine(fadeOutCR);
            layerNum++;
            IEnumerator fadeInCR = fadeIn(gridMaps[layerNum]);
            StartCoroutine(fadeInCR);
        }else if(Input.mouseScrollDelta.y < 0 && (layerNum-1 >= 0)){ //move layer down 
            IEnumerator fadeOutCR = fadeOut(gridMaps[layerNum]);
            StartCoroutine(fadeOutCR);
            layerNum--;
            IEnumerator fadeInCR = fadeIn(gridMaps[layerNum]);
            StartCoroutine(fadeInCR);

        }
    }

    void playerSwitch(){
        if (Input.GetKeyDown("e") && (playerNum+1 < players.Length)){ // move layer up
            if(players[playerNum].transform.parent != players[playerNum+1].transform.parent){
                IEnumerator fadeOutCR = fadeOut(gridMaps[layerNum]);
                StartCoroutine(fadeOutCR);
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum++;
                layerNum = getLayerNum(players[playerNum].transform.parent);
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                IEnumerator fadeInCR = fadeIn(gridMaps[layerNum]);
                StartCoroutine(fadeInCR);
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum++;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            }
        }else if(Input.GetKeyDown("q") && (playerNum-1 >= 0)){ //move layer down 
            if(players[playerNum].transform.parent != players[playerNum-1].transform.parent){
                IEnumerator fadeOutCR = fadeOut(gridMaps[layerNum]);
                StartCoroutine(fadeOutCR);
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum--;
                layerNum = getLayerNum(players[playerNum].transform.parent);
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                IEnumerator fadeInCR = fadeIn(gridMaps[layerNum]);
                StartCoroutine(fadeInCR);
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum--;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            }
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

    IEnumerator fadeIn(Transform layer){
        Color tempColor;
        Vector3Int tempPos;
        LayerControl layerInfo = layer.GetComponent<LayerControl>();
        int xCount = layerInfo.getBounds().xMin;
        int yCount = layerInfo.getBounds().yMin;
        for(float i = 0; i < 1; i += Time.deltaTime*fadeRate){
            //fades in every tile of the layer
            while(yCount <= layerInfo.getBounds().yMax){
                tempPos = new Vector3Int(xCount, yCount, 0);
                if(layer.GetComponent<Tilemap>().HasTile(tempPos)){
                    layer.GetComponent<Tilemap>().SetTileFlags(tempPos, TileFlags.None);
                    tempColor = layer.GetComponent<Tilemap>().GetColor(tempPos);
                    Debug.Log(tempColor);
                    tempColor.a = i;
                    layer.GetComponent<Tilemap>().SetColor(tempPos, tempColor);
                }

                xCount++;
                if(xCount > layerInfo.getBounds().xMax){
                    xCount = layerInfo.getBounds().xMin;
                    yCount++;
                }
            }
            xCount = layerInfo.getBounds().xMin;
            yCount = layerInfo.getBounds().yMin;
            //fades in every child of the layer
            foreach(Transform child in layer){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }
        //ensures that everything has an alpha value of 0 
        while(yCount <= layerInfo.getBounds().yMax){
            tempPos = new Vector3Int(xCount, yCount, 0);
            if(layer.GetComponent<Tilemap>().HasTile(tempPos)){
                layer.GetComponent<Tilemap>().SetTileFlags(tempPos, TileFlags.None);
                tempColor = layer.GetComponent<Tilemap>().GetColor(tempPos);
                tempColor.a = 1;
                layer.GetComponent<Tilemap>().SetColor(tempPos, tempColor);
            }
            //
            xCount++;
            if(xCount > layerInfo.getBounds().xMax){
                xCount = layerInfo.getBounds().xMin;
                yCount++;
            }
        }
        foreach(Transform child in layer){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            child.gameObject.SetActive(true); //deactivates every child of the layer
        }

        //deactivates the layer afterwards so it doesn't interfere with other operations.
        layer.gameObject.SetActive(true);
    }
    IEnumerator fadeOut(Transform layer){
        Color tempColor;
        Vector3Int tempPos;
        LayerControl layerInfo = layer.GetComponent<LayerControl>();
        int xCount = layerInfo.getBounds().xMin;
        int yCount = layerInfo.getBounds().yMin;
        for(float i = 1; i > 0; i -= Time.deltaTime*fadeRate){
            //fades every tile of the layer
            while(yCount <= layerInfo.getBounds().yMax){
                //place stuff here
                tempPos = new Vector3Int(xCount, yCount, 0);
                if(layer.GetComponent<Tilemap>().HasTile(tempPos)){
                    layer.GetComponent<Tilemap>().SetTileFlags(tempPos, TileFlags.None);
                    tempColor = layer.GetComponent<Tilemap>().GetColor(tempPos);
                    tempColor.a = i;
                    layer.GetComponent<Tilemap>().SetColor(tempPos, tempColor);
                }
                //
                xCount++;
                if(xCount > layerInfo.getBounds().xMax){
                    xCount = layerInfo.getBounds().xMin;
                    yCount++;
                }
            }
            xCount = layerInfo.getBounds().xMin;
            yCount = layerInfo.getBounds().yMin;

            //fades every child of the layer
            foreach(Transform child in layer){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }

        //ensures that everything has an alpha value of 0 
        while(yCount <= layerInfo.getBounds().yMax){
            tempPos = new Vector3Int(xCount, yCount, 0);
            if(layer.GetComponent<Tilemap>().HasTile(tempPos)){
                layer.GetComponent<Tilemap>().SetTileFlags(tempPos, TileFlags.None);
                tempColor = layer.GetComponent<Tilemap>().GetColor(tempPos);
                tempColor.a = 0;
                layer.GetComponent<Tilemap>().SetColor(tempPos, tempColor);
            }
            //
            xCount++;
            if(xCount > layerInfo.getBounds().xMax){
                xCount = layerInfo.getBounds().xMin;
                yCount++;
            }
        }
        foreach(Transform child in layer){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            child.gameObject.SetActive(false); //deactivates every child of the layer
        }

        //deactivates the layer afterwards so it doesn't interfere with other operations.
        layer.gameObject.SetActive(false);
    }
    
    IEnumerator testOut(Transform player){
        //Debug.Log("Coroline testout called");
        Color tempColor = player.GetComponent<SpriteRenderer>().color;
        for(float i = 1f; i > 0f; i -= Time.deltaTime*fadeRate){
            tempColor.a = i;
            player.GetComponent<SpriteRenderer>().color = tempColor;
            yield return null;
        }
        //yield return new WaitForSeconds(.5f);
    }
    IEnumerator testIn(Transform player){
        //Debug.Log("Coroline testIn called");
        Color tempColor = player.GetComponent<SpriteRenderer>().color;
        for(float i = 0f; i < 1f; i += Time.deltaTime*fadeRate){
            tempColor.a = i;
            player.GetComponent<SpriteRenderer>().color = tempColor;
             yield return null;
        }
        //yield return new WaitForSeconds(5f);
    }  
}
