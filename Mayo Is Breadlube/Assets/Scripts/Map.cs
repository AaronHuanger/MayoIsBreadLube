using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Transform[] layers; //stores the tile maps of the grid object 
    GameObject[] players;
    
    public GameObject[] getPlayerGameObjects(){
        return players;
    }
    public Transform[] getLayerTransforms(){
        return layers;
    }
    //Controls: mouse scrollbar --> shifts layesr
    //          shift + mouse scroll --> shifts player
    void Awake()
    {   
        //get array of player from entire
        players = transform.GetComponentInParent<Level>().getPlayerGameObjects();

        //get array of layer transform
        int numChild = transform.childCount;
        layers = new Transform[numChild];
        for(int i = 0; i < numChild; i++){
            layers[i] = transform.GetChild(i);
        }

    }
}
