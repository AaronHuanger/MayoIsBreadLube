using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileLibrary
{
    public static void toAllTiles(Transform layer, System.Action action){
        Tilemap layerMap = layer.GetComponent<Tilemap>();
        Layer layerInfo = layer.GetComponent<Layer>();
        Vector3Int tempPos;

        int xCount = layerInfo.getBounds().xMin;
        int yCount = layerInfo.getBounds().yMin;

        while(yCount <= layerInfo.getBounds().yMax){
            tempPos = new Vector3Int(xCount , yCount , 0);
            if(layerMap.HasTile(tempPos)){
                action();
            }
            //changes y position of the tilemap
            xCount++;
            if(xCount > layerInfo.getBounds().xMax){
                xCount = layerInfo.getBounds().xMin;
                yCount++;
            }
        }
    }
}
