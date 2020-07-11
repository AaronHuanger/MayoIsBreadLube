using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for layers - ex: sky, ground, underground 
abstract public class Layer
{
    public Tile[][] tileMap;
    List<Player> players;
    List<Enemies> enemies; 
}
