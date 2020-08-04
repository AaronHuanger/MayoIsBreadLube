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


    bool shiftFinished = true;
    

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
        Color tempColor;
        for(int i = 0; i < layers.Length; i++){
            layers[i].GetComponent<Layer>().changeOpacity(0);
            foreach(Transform child in layers[i]){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = 0;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
        }
        //make the layer with the starting player visible
        layers[layerNum].GetComponent<Layer>().changeOpacity(1);
        foreach(Transform child in layers[layerNum]){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1;
            child.GetComponent<SpriteRenderer>().color = tempColor;
        }
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
    if(shiftFinished)
        if (Input.mouseScrollDelta.y > 0 && (layerNum+1 < layers.Length)){ // move layer up
            layerChange(layers[layerNum], layerNum+1);
        }else if(Input.mouseScrollDelta.y < 0 && (layerNum-1 >= 0)){ //move layer down 
            layerChange(layers[layerNum], layerNum-1);
        }
    }

    void playerSwitch(){ //the controls for switching the view between players
    if(shiftFinished)
        if (Input.GetKeyDown("e") && (playerNum+1 < players.Length)){ // move layer up
            if(players[playerNum].transform.parent != players[playerNum+1].transform.parent){
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum++;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                layerChange(layers[layerNum], players[playerNum].transform.parent);
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                players[playerNum].GetComponent<CharacterMovement>().enabled = false;
                playerNum++;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                players[playerNum].GetComponent<CharacterMovement>().enabled = true;
            }
        }else if(Input.GetKeyDown("q") && (playerNum-1 >= 0)){ //move layer down 
            if(players[playerNum].transform.parent != players[playerNum-1].transform.parent){
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                playerNum--;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                layerChange(layers[layerNum], players[playerNum].transform.parent);
                
            }else{
                players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
                players[playerNum].GetComponent<CharacterMovement>().enabled = false;
                playerNum--;
                players[playerNum].GetComponent<SpriteRenderer>().color = outline;
                players[playerNum].GetComponent<CharacterMovement>().enabled = true;
            }
        }
    }

    public void layerChange(Transform layer1, Transform layer2){
        StartCoroutine(fadeOutLayer(layer1));
        //StartCoroutine(fadeLayers(layer1, layer2));
        layerNum = getLayerNum(layer2);
        StartCoroutine(fadeInLayer(layer2));
    }
    public void layerChange(Transform layer, int nextLayerNum){
        StartCoroutine(fadeOutLayer(layer));
        //StartCoroutine(fadeLayers(layer, layers[nextLayerNum]));
        layerNum = nextLayerNum;
        StartCoroutine(fadeInLayer(layers[layerNum]));
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
    private IEnumerator fadeInLayer(Transform layer){
        shiftFinished = false;

        Color tempColor;

        //activates the layer
        layer.gameObject.SetActive(true);
        //changeOpacity(0);
        for(float i = 0; i < 1; i += Time.deltaTime*fadeRate){
            //fadein every tile of the layer
            layer.GetComponent<Layer>().changeOpacity(i);
            //fadein every child of the layer
            foreach(Transform child in layer){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }
        //ensures that everything has an alpha value of 1 
        layer.GetComponent<Layer>().changeOpacity(1);

        foreach(Transform child in layer){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            child.gameObject.SetActive(true); //deactivates every child of the layer
        }

        shiftFinished = true;
    }
    private IEnumerator fadeOutLayer(Transform layer){
        shiftFinished = false; 

        Color tempColor;
        for(float i = 1; i > 0; i -= Time.deltaTime*fadeRate){
            //fades every tile of the layer
            layer.GetComponent<Layer>().changeOpacity(i);
            //fades every child of the layer
            foreach(Transform child in layer){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = i;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            yield return null;
        }

        //ensures that everything has an alpha value of 0 
        layer.GetComponent<Layer>().changeOpacity(0);

        foreach(Transform child in layer){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0;
            child.GetComponent<SpriteRenderer>().color = tempColor;
            //child.gameObject.SetActive(false); //deactivates every child of the layer
            //Note child objects are automatically deactivated 
        }
        //deactivates the layer afterwards so it doesn't interfere with other operations.
        layer.gameObject.SetActive(false);

        shiftFinished = true;
    }
}