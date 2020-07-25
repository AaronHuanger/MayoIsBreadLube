using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public int baseDamage;
    //protected int[][] coordinates;
    public Ability(){}

    //single target
    public virtual void activate(Unit target){}

    //aoe
    public virtual void activate(){}

    //helpers
    protected void affectTiles(Damage damage, int[][] coordinates){
        for(int i = 0; i < coordinates.length; i++){
            //get a tile matching coordinates in coordinates
            for(int j = 0; j < /*number of stuff on that tile*/){
                if(/*friendly fire check*/){
                    /*unit.takeDamage(damage)*/
                }
            }
        }
    }

    protected void affectTiles(string status, int[][] coordinates){

    }

    protected void affectTiles(Damage damage, string status, int[][] coordinates){

    }

}
