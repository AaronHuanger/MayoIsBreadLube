using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    Layer[] layers; //stores the tile maps of the grid object 
    int layerNum;
    public Transform startingLayer;
    
    void Awake()
    {           
        //get array of layer scripts
        int numChild = transform.childCount;
        layers = new Layer[numChild];
        for(int i = 0; i < numChild; i++){
            layers[i] = transform.GetChild(i).GetComponent<Layer>();
        }

        //set layerNum to be equal to starting layer - preparation for shift control
        layerNum = getLayerIndex(startingLayer);
        Debug.Log(layers.Length);
        Debug.Log(layerNum);
    }

    void Start(){
        //cause the rest of the layers to become invisible
        Color tempColor;
        for(int i = 0; i < layers.Length; i++){
            layers[i].changeOpacity(0);
            foreach(Transform child in layers[i].transform){
                tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = 0;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
        }

        //make starting layer visible
        reveal(startingLayer.GetComponent<Layer>());
    }
    public void hide(Layer layer){
        Color tempColor;
        layer.changeOpacity(0);
        foreach(Transform child in layer.transform){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 0;
            child.GetComponent<SpriteRenderer>().color = tempColor;
        }
    }

    public void reveal(Layer layer){
        Color tempColor;
        layer.changeOpacity(1);
        foreach(Transform child in layer.transform){
            tempColor = child.GetComponent<SpriteRenderer>().color;
            tempColor.a = 1;
            child.GetComponent<SpriteRenderer>().color = tempColor;
        }

    }
    
    public Layer[] getLayers(){
        return layers;
    }

    public int getLayerNum(){
        return layerNum;
    }
    public void setLayerNum(int index){
        layerNum = index;
    }
    public Layer getLayer(int index){
        return layers[index];
    }
    public Layer getCurLayer(){
        return layers[layerNum];
    }
    public void setLayerNum(Transform layer){
        layerNum = getLayerIndex(layer);
    }
    int getLayerIndex(Transform layer){
        for(int i = 0; i < transform.childCount; i++){
            if(layers[i].transform == layer){
                return i;
            }
        }
        Debug.LogError("Layer doesn't exist. Failed to return layerindex");
        return -1; // crashes program. 
    }
}