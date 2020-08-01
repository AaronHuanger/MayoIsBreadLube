using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shift controller for layers and players 
class ShiftControl : MonoBehaviour
{
    public GameObject startingPlayer;
    public float fadeRate = 0.5f;
    public Color outline = UnityEngine.Color.red;


    public Map map;    
    Transform[] layers;
    int layerNum;
    GameObject[] players;
    int playerNum;
    

    // Start is called before the first frame update
    void Start()
    {   
        layers = map.getLayerTransforms(); // gets layers from map script
        players = map.getPlayerGameObjects(); // gets players from map script

        //get starting player number 
        for(int i = 0; i < players.Length; i++){
            if(players[i] == startingPlayer){
                playerNum = i;
                break;
            }
        }
        //highlight starting player with the outline color
        startingPlayer.gameObject.GetComponent<SpriteRenderer>().color = outline;

        //get layer number based off 
        layerNum = getLayerNum(startingPlayer.transform.parent);


        //cause the rest of the layers to become invisible
        for(int i = 0; i < layers.Length; i++){
            IEnumerator fadeOutCR = layers[i].GetComponent<Layer>().fadeOut();
            StartCoroutine(fadeOutCR);
        }
        //make the layer with the starting player visible
        IEnumerator fadeInCR = layers[layerNum].GetComponent<Layer>().fadeIn();
        StartCoroutine(fadeInCR);
    }

    // Update is called once per frame
    void Update()
    {
        layerSwitch();
        playerSwitch();
    }

    void errorCheck(){
        if(!startingPlayer.CompareTag("Player")){
            Debug.LogError("A player was not placed in startingLayer");
        }
    }
    
    void layerSwitch(){ //the controls for switching views between layers 
        if (Input.mouseScrollDelta.y > 0 && (layerNum+1 < layers.Length)){ // move layer up
            /*IEnumerator fadeOutCR = map.fadeOut(layers[layerNum]);
            StartCoroutine(fadeOutCR);
            layerNum++;
            IEnumerator fadeInCR = map.fadeIn(layers[layerNum]);
            StartCoroutine(fadeInCR);*/
            layerChange(layers[layerNum], layerNum+1);
        }else if(Input.mouseScrollDelta.y < 0 && (layerNum-1 >= 0)){ //move layer down 
            /*IEnumerator fadeOutCR = map.fadeOut(layers[layerNum]);
            StartCoroutine(fadeOutCR);
            layerNum--;
            IEnumerator fadeInCR = map.fadeIn(layers[layerNum]);
            StartCoroutine(fadeInCR);*/
            layerChange(layers[layerNum], layerNum-1);
        }
    }

    void playerSwitch(){ //the controls for switching the view between players
        if (Input.GetKeyDown("e") && (playerNum+1 < players.Length)){ // move layer up
            if(players[playerNum].transform.parent != players[playerNum+1].transform.parent){
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum++;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                layerChange(layers[layerNum], players[playerNum].transform.parent);
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum++;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            }
        }else if(Input.GetKeyDown("q") && (playerNum-1 >= 0)){ //move layer down 
            if(players[playerNum].transform.parent != players[playerNum-1].transform.parent){
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum--;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                layerChange(layers[layerNum], players[playerNum].transform.parent);
                
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum--;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            }
        }
    }

    public void layerChange(Transform layer1, Transform layer2){
        IEnumerator fadeOutCR = layers[layerNum].GetComponent<Layer>().fadeOut();
        StartCoroutine(fadeOutCR);
        layerNum = getLayerNum(layer2);
        IEnumerator fadeInCR = layers[layerNum].GetComponent<Layer>().fadeIn();
        StartCoroutine(fadeInCR);
    }
    public void layerChange(Transform layer, int nextLayerNum){
        IEnumerator fadeOutCR = layers[layerNum].GetComponent<Layer>().fadeOut();
        StartCoroutine(fadeOutCR);

        layerNum = nextLayerNum;

        IEnumerator fadeInCR = layers[layerNum].GetComponent<Layer>().fadeIn();
        StartCoroutine(fadeInCR);
    }

    int getLayerNum(Transform layer){
        for(int i = 0; i < transform.childCount; i++){
            if(layers[i] == layer){
                return i;
            }
        }
        Debug.LogError("Layer doesn't exist. Failed to return layerindex");
        return -1; // crashes program. 
    }
}
