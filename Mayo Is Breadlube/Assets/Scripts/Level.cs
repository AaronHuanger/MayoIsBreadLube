using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    Transform[] maps;
    public GameObject[] players;

    void Awake(){
        //gets player object from the entire scene
        players = GameObject.FindGameObjectsWithTag("Player");
        
        //gets the map transform in this level
        int numChild = transform.childCount;
        maps = new Transform[numChild];
        for(int i = 0; i < numChild; i++){
            maps[i] = transform.GetChild(i);
        }
    }
    public GameObject[] getPlayerGameObjects(){
        return players;
    }
}