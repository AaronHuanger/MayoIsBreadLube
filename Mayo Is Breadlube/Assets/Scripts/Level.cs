using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //Transform[] maps;
    Map[] maps;
    public GameObject[] players;

    void Awake(){
        //gets player object from the entire scene
        players = GameObject.FindGameObjectsWithTag("Player");
        //gets the map transform in this level
        /*int numChild = transform.childCount;
        maps = new Transform[numChild];
        for(int i = 0; i < numChild; i++){
            maps[i] = transform.GetChild(i);
        }*/
        
        //get the map scripts in this level
        int numChild = transform.childCount;
        maps = new Map[numChild];
        for(int i = 0; i < numChild; i++){
            maps[i] = transform.GetChild(i).GetComponent<Map>();
        }
    }
    void Start(){
        
    }
    public GameObject[] getPlayerGameObjects(){
        return players;
    }
    public Map[] getMaps(){
        return maps;
    }
}