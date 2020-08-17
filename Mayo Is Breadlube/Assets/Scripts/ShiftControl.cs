using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftControl : MonoBehaviour
{

    public Color outline = UnityEngine.Color.red;

    //mapShift related
    public Transform startingMap;
    public Map[] maps; // contains layernum & initializes invisible
    int mapNum;

    //playerShift related
    public GameObject startingPlayer;
    GameObject[] players;
    int playerNum;

    //fade related
    public float fadeRate = 0.5f;
    bool shiftFinished = true;

    void Awake(){
        //--Map Initialization
        //get map array
        maps = new Map[transform.childCount];
        for(int i = 0; i < maps.Length; i++){
            maps[i] = transform.GetChild(i).GetComponent<Map>();
        }
        //set map num
        mapNum = getMapNum(startingMap);

    }

    void Start()
    {
        //--Map Initialization
        //make every map inactive. Placed here because shiftcontrol is initialized before map. Inactive object's Awake() is not called
        for(int i = 0; i < maps.Length; i++){
            maps[i].gameObject.SetActive(false);
        }
        //make the first map active
        maps[mapNum].gameObject.SetActive(true);

        //--Player Initialization
        //get player array
        players = transform.GetComponent<Level>().getPlayerGameObjects();
        //set player num
        playerNum = getPlayerNum(startingPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(shiftFinished){
            mapShiftControl();
            layerShiftControl();
            playerShiftControl();
        }
    }

    public void playerShiftControl(){
        if(Input.GetKeyDown("c") && playerNum + 1 < players.Length){
            players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
            playerNum++;
            players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            if(players[playerNum - 1].transform.parent != players[playerNum].transform.parent){ // if player is on the same layer as the next player object. don't do fade effects
                mapChange(maps[mapNum], players[playerNum].transform.parent.parent);
                layerChange(maps[mapNum].getCurLayer().transform, players[playerNum].transform.parent);
            }               
        }else if(Input.GetKeyDown("z") && playerNum - 1 >= 0){
            players[playerNum].GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
            playerNum--;
            players[playerNum].GetComponent<SpriteRenderer>().color = outline;
            if(players[playerNum + 1].transform.parent != players[playerNum].transform.parent){
                mapChange(maps[mapNum], players[playerNum].transform.parent.parent);
                layerChange(maps[mapNum].getCurLayer().transform, players[playerNum].transform.parent);
            }
        }
    }

    public void layerShiftControl(){
        if(Input.GetKeyDown("e") && maps[mapNum].getLayerNum() + 1 < maps[mapNum].getLayers().Length){
            layerChange(maps[mapNum].getCurLayer().transform, maps[mapNum].getLayerNum()+1);
        }else if(Input.GetKeyDown("q") && maps[mapNum].getLayerNum() - 1 >= 0){
            layerChange(maps[mapNum].getCurLayer().transform, maps[mapNum].getLayerNum()-1);
        }
    }
    
    public void mapShiftControl(){ //assumes that a map's current layer doesn't change while it is inactive
        if(Input.GetKeyDown("3") && mapNum + 1 < maps.Length){
            mapChange(maps[mapNum], maps[mapNum+1].transform);
        }else if(Input.GetKeyDown("1") && mapNum - 1 >= 0){
            mapChange(maps[mapNum], maps[mapNum-1].transform);
        }
    }


    public void layerChange(Transform layer1, Transform layer2){
        StartCoroutine(fadeOutLayer(layer1));
        //StartCoroutine(fadeLayers(layer1, layer2));
        maps[mapNum].setLayerNum(layer2);
        StartCoroutine(fadeInLayer(layer2));
    }
    public void layerChange(Transform layer, int nextLayerNum){
        StartCoroutine(fadeOutLayer(layer));
        //StartCoroutine(fadeLayers(layer, layers[nextLayerNum]));
        maps[mapNum].setLayerNum(nextLayerNum);
        StartCoroutine(fadeInLayer(maps[mapNum].getLayer(nextLayerNum).transform));
    }

    public void mapChange(Transform map1, Transform map2){
        Map tempMap = map1.GetComponent<Map>();
        StartCoroutine(fadeOutLayer(tempMap.getCurLayer().transform));
        mapNum = getMapNum(map2);
        maps[mapNum].getCurLayer().changeOpacity(0); //may consider changing this if a map's current layer is changed while it is inactive
        maps[mapNum].gameObject.SetActive(true);
        StartCoroutine(fadeInLayer(maps[mapNum].getCurLayer().transform));
                
        //makes it so that the original map doesn't deactive until the coroutine is over
        while(shiftFinished){
            tempMap.gameObject.SetActive(false);
            tempMap.getCurLayer().changeOpacity(1);
        }
    }

    public void mapChange(Map map1, Transform map2){
        StartCoroutine(fadeOutLayer(map1.getCurLayer().transform));
        mapNum = getMapNum(map2);
        maps[mapNum].getCurLayer().changeOpacity(0); //may consider changing this if a map's current layer is changed while it is inactive
        maps[mapNum].gameObject.SetActive(true);
        StartCoroutine(fadeInLayer(maps[mapNum].getCurLayer().transform));
                
        //makes it so that the original map doesn't deactive until the coroutine is over
        while(shiftFinished){
            map1.gameObject.SetActive(false);
            map1.getCurLayer().changeOpacity(1);
        }
    }
    private IEnumerator fadeInLayer(Transform layer){
        shiftFinished = false;

        Color tempColor;

        //activates the layer
        //layer.gameObject.SetActive(true);
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
            //child.gameObject.SetActive(true); //deactivates every child of the layer
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
        //layer.gameObject.SetActive(false);

        shiftFinished = true;
    }
    public int getPlayerNum(GameObject player){
        for(int i = 0; i < players.Length; i++){
            if(player == players[i]){
                return i;
            }
        }
        Debug.LogError("Failed to find player");
        return -1;
        }

    public int getMapNum(Transform map){
        for(int i = 0; i < maps.Length; i++){
            if(map == maps[i].transform){
                return i;
            }
        }
        Debug.LogError("Failed to find map");
        return -1;
    }
}
