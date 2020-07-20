using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int value;
    public string type;
    public Unit source;

    public Damage(int v, string t, Unit s){
        value = v;
        type = t;
        source = s;
    }
}
