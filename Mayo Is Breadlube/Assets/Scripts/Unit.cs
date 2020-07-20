using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    final float pierceMult = 2.5;
    final float bluntMult = 1.3;
    final float damageCap = 99999;
    public int actionPoints;
    public int maxHp;
    public int maxArmor;

    public int hp;
    public int armor;

    public bool guard;
    public bool parry;
    public float hpMult;
    public int atkMult;

    //public List<string> statuses;

    //If theres enough probably worth making them into an array
    public int poisonCounter;
    public int stunCounter;
    public int staggerCounter;
    
    //public string weapon;

    public AbilityTree abilityTree;

    public int takeNormalDamage(Damage damage){
        if(damage.type = "pierce"){
            if(armor > 0){
                int calcArmor = damage.value / (hpMult * pierceMult);
                int remainder = mathf.Clamp(damage.value - calcArmor, 0, damageCap);

                armor = mathf.Clamp((calcArmor - damage.value) * (hpMult * pierceMult), 0, maxArmor);
                health = mathf.Clamp(health - remainder, 0, maxHp);
            }else{
                health = mathf.Clamp(health - damage.value, 0, maxHp);
            }
        }else if(damage.type = "blunt"){
            if(armor > 0){
                int remainder = mathf.Clamp(damage.value - armor, 0, damageCap);

                armor = mathf.Clamp(armor - damage.value, 0, maxArmor);
                health = mathf.Clamp(health - (remainder * bluntMult), 0, maxHp);
            }else{
                health = mathf.Clamp(health - damage.value, 0, maxHp);
            }
        }else if(damage.type = "normal"){
            if(armor > 0){
                int remainder = mathf.Clamp(damage.value - armor, 0, damageCap);
                
                armor = mathf.Clamp(armor - damage.value, 0, maxArmor);
                health = mathf.Clamp(health - remainder, 0, maxHp);
            }else{
                health = mathf.Clamp(health - damage.value, 0, maxHp);
            }
        }
    }
}
