using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for layers - ex: sky, ground, underground 
abstract public class Layer : MonoBehaviour
{
    public MyTileData[,] tileMap;
    List<Player> players;
    List<Enemy> enemies; 
}
